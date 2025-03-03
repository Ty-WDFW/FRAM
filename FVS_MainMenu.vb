Imports System.Data.OleDb
Imports System.Data.SQLite
Imports System.Data
Imports System.IO
Imports System.Windows
Imports System.Text
Imports System.IO.File

Public Class FVS_MainMenu
    Public RunID(10000) As Integer
    Public BaseID(10000) As Integer
    Public RunIDName(10000) As String
    Public RunBasePeriodID(10000) As Integer
    Public PrnLine, PrnPart As String
    Public rssw As StreamWriter


    Private Sub FVS_MainMenu_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ReadOldCmd = False
        FramVersionLabel.Text = "Version " & FramVersion
        FormHeight = 762
        FormWidth = 832
        If DevWidth > My.Computer.Screen.Bounds.Width Then
            FormWidthScaler = DevWidth / My.Computer.Screen.Bounds.Width
        Else
            FormWidthScaler = 1
        End If
        'FormWidthScaler = 1280 / My.Computer.Screen.Bounds.Width
        '- Check if Form fits within Screen Dimensions
        If (FormHeight > My.Computer.Screen.Bounds.Height Or
            FormWidth > My.Computer.Screen.Bounds.Width) Then
            Me.Height = FormHeight / (DevHeight / My.Computer.Screen.Bounds.Height)
            Me.Width = FormWidth / (DevWidth / My.Computer.Screen.Bounds.Width)
            If FVS_MainMenu_ReSize = False Then
                Resize_Form(Me)
                FVS_MainMenu_ReSize = True
            End If
        End If
        BackFramSave = False
    End Sub

    Private Sub FVS_Exit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles FVS_Exit.Click
        Dim Result As Integer
        If ChangeAnyInput = True Or ChangeBackFram = True Or ChangeFishScalers = True Or
           ChangeNonRetention = True Or ChangePSCMaxER = True Or ChangeSizeLimit = True Or
           ChangeStockFishScaler = True Or ChangeStockRecruit = True Then
            ChangeAnyInput = True
            Result = MsgBox("Input Values have been Changed!" & vbCrLf & "Save Current Model Run ???", MsgBoxStyle.YesNo)
            If Result = vbYes Then
                Call SaveModelRunInputs()

            End If
        End If

        End

    End Sub

    Private Sub OpenDB_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles OpenDB.Click

        Dim OpenFVSdatabase As New OpenFileDialog()
        'Dim FVSdatabaseBackupName As String
        'Dim DBNameLen As Integer
        Dim Result As Integer

        If ChangeAnyInput = True Or ChangeBackFram = True Or ChangeFishScalers = True Or
           ChangeNonRetention = True Or ChangePSCMaxER = True Or ChangeSizeLimit = True Or
           ChangeStockFishScaler = True Or ChangeStockRecruit = True Then
            ChangeAnyInput = True
            Result = MsgBox("Input Values have been Changed!" & vbCrLf & "Save Current Model Run ???", MsgBoxStyle.YesNo)
            If Result = vbYes Then
                Call SaveModelRunInputs()
            End If
        End If

TryDBAgain:
        FVSdatabasename = ""
        OpenFVSdatabase.Filter = "DataBase Files (*.db)|*.db|All files (*.*)|*.*"
        OpenFVSdatabase.FilterIndex = 1
        OpenFVSdatabase.RestoreDirectory = True
        If OpenFVSdatabase.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
            Try
                FVSdatabasename = OpenFVSdatabase.FileName
                FVSshortname = My.Computer.FileSystem.GetFileInfo(FVSdatabasename).Name
                FVSdatabasepath = My.Computer.FileSystem.GetFileInfo(FVSdatabasename).DirectoryName
            Catch Ex As Exception
                MessageBox.Show("Cannot read file from disk. Original error: " & Ex.Message)
            End Try
        End If
        If InStr(FVSdatabasename, "NewTransferModelRun.db") > 0 Or
         InStr(FVSdatabasename, "ModelRunTransfer.db") > 0 Then
            MsgBox("You cannot use the NewTransferModelRun or ModelRunTransfer" & vbCrLf &
         "Databases because they are reserved for Run Transfer Operations" & vbCrLf &
         "Please chose another Database", MsgBoxStyle.OkOnly)
            GoTo TryDBAgain
        End If
        If FVSdatabasename.Length > 50 Then
            DatabaseNameLabel.Text = FVSshortname
        Else
            DatabaseNameLabel.Text = FVSdatabasename
        End If
        If FVSdatabasename = "" Then Exit Sub

        ''- Auto Save Backup-Copy of Database File
        'Me.Cursor = Cursors.WaitCursor
        'DBNameLen = InStr(FVSdatabasename, "mdb")
        'FVSdatabaseBackupName = FVSdatabasename.Substring(0, DBNameLen - 2) & "_AutoBackup.mdb"
        'If Exists(FVSdatabaseBackupName) Then Delete(FVSdatabaseBackupName)
        'File.Copy(FVSdatabasename, FVSdatabaseBackupName, True)
        'Me.Cursor = Cursors.Default

        '- DB Connection String
        FramDB.ConnectionString = "Data Source=" & FVSdatabasename & ";Version=3;Compress=True;"
        Me.Visible = False



        '==============================================================================================
        '- (Pete 12/13) Code that checks for the existence of the Target Sublegal:Legal Ratio and 
        '  and RunEncounterRateAdjustment table (SLRatio) 
        '- needed to use external sublegals; works to make things functional retroactively
        Dim sql As String       'SQL Query text string
        Dim SQLiteAdabater As SQLite.SQLiteDataAdapter

        'First check the FRAM database for the SLRatio and RunEncounterRateAdjustment tables
        FramDB.Open()
        Dim restrictions1(3) As String
        Dim restrictions2(3) As String
        Dim DoesTableExist1 As Boolean
        Dim DoesTableExist2 As Boolean
        restrictions1(2) = "SLRatio"

        Dim dbTbl As DataTable = FramDB.GetSchema("Tables", restrictions1)
        If dbTbl.Rows.Count = 0 Then
            'Table does not exist
            DoesTableExist1 = False
        Else
            'Table exists
            DoesTableExist1 = True
        End If

        dbTbl.Dispose()
        FramDB.Close()

        'If SLRatio doesn't exist, create it.
        If DoesTableExist1 = False Then
            sql = "CREATE TABLE SLRatio (RunID INTEGER,FisheryID INTEGER,Age INTEGER,TimeStep INTEGER,TargetRatio DOUBLE, RunEncounterRateAdjustment DOUBLE, UpdateWhen DATETIME, UpdateBy VARCHAR(255))"
            'Now connect to the database and make the table...
            'create a command
            Dim my_Command As New SQLite.SQLiteCommand(sql, FramDB)
            FramDB.Open()
            'command execute
            my_Command.ExecuteNonQuery()
            FramDB.Close()
        End If

        '==============================================================================================

        '- Recordset (Model Run) Selection
        RecordsetSelectionType = 1
        FVS_ModelRunSelection.ShowDialog()
        Me.Visible = True
        Me.BringToFront()
        RecordSetNameLabel.Text = RunIDNameSelect
        'If RunIDSelect = 0 Then
        ' FVSdatabasename = "" 'Disabled so batch run will work - Ty
        'MsgBox("NO Recordsets Available in this Database File" & vbCrLf & _
        '"You must Read Old CMD File to continue", MsgBoxStyle.OkOnly)
        'End If

    End Sub

    Private Sub InputOptions_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If FVSdatabasename = "" Then
            MsgBox("Database and Model Run Must be Selected First !!!", MsgBoxStyle.OkOnly)
            Exit Sub
        End If
        Me.Visible = False
        'FVS_InputMenu.ShowDialog()
        Me.BringToFront()
    End Sub

    Private Sub ModelRun_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ModelRun.Click
        Dim Result As Integer
        If FVSdatabasename = "" Then
            MsgBox("Database and Model Run Must be Selected First !!!", MsgBoxStyle.OkOnly)
            Exit Sub
        End If
        If ChangeAnyInput = True Or ChangeBackFram = True Or ChangeFishScalers = True Or
          ChangeNonRetention = True Or ChangePSCMaxER = True Or ChangeSizeLimit = True Or
          ChangeStockFishScaler = True Or ChangeStockRecruit = True Then
            ChangeAnyInput = True
            Result = MsgBox("Input Values have been Changed!" & vbCrLf & "Changes Must be Saved before Running Model!!!" & vbCrLf & "Save Current Model Run ???", MsgBoxStyle.YesNo)
            If Result = vbYes Then
                'Call SaveModelRunInputs()
                Me.Visible = False
                FVS_SaveModelRunInputs.ShowDialog()
                Me.Visible = True
                RecordSetNameLabel.Text = RunIDNameSelect
                Me.BringToFront()
            Else
                MsgBox("Please be aware that the OUTPUT for this run" & vbCrLf & "cannot be duplicated without saving your INPUT values", MsgBoxStyle.OkOnly)
            End If
        End If
        Me.Visible = False
        FVS_RunModel.ShowDialog()
        Me.BringToFront()
    End Sub

    Private Sub FramUtilButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If FVSdatabasename = "" Then
            MsgBox("Database and Model Run Must be Selected First !!!", MsgBoxStyle.OkOnly)
            Exit Sub
        End If
        Me.Visible = False

        'FVS_FramUtils.ShowDialog()
        RecordSetNameLabel.Text = RunIDNameSelect
        Me.BringToFront()
    End Sub

    Private Sub OutputResults_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If FVSdatabasename = "" Then
            MsgBox("Database and Model Run Must be Selected First !!!", MsgBoxStyle.OkOnly)
            Exit Sub
        End If
        Me.Visible = False
        'FVS_Output.ShowDialog()
        Me.Refresh()
        Me.BringToFront()
    End Sub

    Private Sub PostSeason_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If FVSdatabasename = "" Then
            MsgBox("Database and Model Run Must be Selected First !!!", MsgBoxStyle.OkOnly)
            Exit Sub
        End If
        Me.Visible = False
        'FVS_BackwardsFram.ShowDialog()
        Me.BringToFront()
    End Sub

    Private Sub SelectRecordset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SelectRecordset.Click
        If FVSdatabasename = "" Then
            MsgBox("Database and Model Run Must be Selected First !!!", MsgBoxStyle.OkOnly)
            Exit Sub
        End If
        RecordsetSelectionType = 1
        Me.Visible = False
        FVS_ModelRunSelection.ShowDialog()
        Me.Visible = True
        Me.BringToFront()
        RecordSetNameLabel.Text = RunIDNameSelect
        If RunIDSelect = 0 Then
            FVSdatabasename = ""
        End If
    End Sub

    Private Sub SaveInputButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If FVSdatabasename = "" Then
            MsgBox("Database and Model Run Must be Selected First !!!", MsgBoxStyle.OkOnly)
            Exit Sub
        End If
        If ChangeAnyInput = True Or ChangeBackFram = True Or ChangeFishScalers = True Or
           ChangeNonRetention = True Or ChangePSCMaxER = True Or ChangeSizeLimit = True Or
           ChangeStockFishScaler = True Or ChangeStockRecruit = True Or AnyChange = True Then
            ChangeAnyInput = True
        Else
            MsgBox("No Input Values have been Changed!" & vbCrLf & "No Action Taken", MsgBoxStyle.OkOnly)
            Exit Sub
        End If

        Me.Visible = False
        FVS_SaveModelRunInputs.ShowDialog()
        Me.Visible = True
        RecordSetNameLabel.Text = RunIDNameSelect
        Me.BringToFront()

    End Sub

    Private Sub VersionChangesButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.Visible = False
        'FVS_VersionChanges.ShowDialog()
        Me.Visible = True
        Me.BringToFront()
    End Sub

    Private Sub RecordSetNameLabel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RecordSetNameLabel.Click

    End Sub

    Private Sub OpenFVSdatabase_FileOk(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles OpenFVSdatabase.FileOk

    End Sub

    Private Sub RunBatch_Click(sender As Object, e As EventArgs) Handles RunBatch.Click
        Call LoadRunArrays()

        Me.Label2.Visible = True
        Me.Label2.Text = "Model Run 1 of " & RunID.Length

        Dim startTime = DateTime.Now
        MSFBiasFlag = True
        ' main loop
        For i = 0 To UBound(RunID)
            Me.Label2.Text = "Model Run " & i + 1 & " of " & RunID.Length
            Me.Label2.Refresh()
            Me.RecordSetNameLabel.Text = RunIDName(i)
            Me.RecordSetNameLabel.Refresh()
            GetRunVariables(RunBasePeriodID(i), RunID(i))
            'Threading.Thread.Sleep(1000)

            Call RunCalcs()

            If i = UBound(RunID) Then
                Dim endTime = DateTime.Now
                Dim duration = endTime - startTime
                Me.Label2.Text = "Done! Duration: " & duration.Seconds & "seconds."
                Me.Label2.Refresh()
                Me.RecordSetNameLabel.Text = "Done!"
                Me.RecordSetNameLabel.Refresh()
            End If

            Application.DoEvents()
        Next
    End Sub

    Public Sub GetRunVariables(ByVal BaseNum As Integer, ByVal RunNum As Integer)

        Dim FramDA As New System.Data.SQLite.SQLiteDataAdapter
        Dim CmdStr As String
        Dim RunIDNum, RecNum, FoundBaseID, Result As Integer
        Dim drd1 As SQLite.SQLiteDataReader
        Dim RunYear As String
        Dim cmd1 As New SQLite.SQLiteCommand()


        Me.Cursor = Cursors.WaitCursor

        ModelRunBPSelect = False

        '- Set Common Variables for Pre-Terminal and Terminal States
        PTerm = 0
        Term = 1

        '- Open Text File for RunTime Messages
        File_Name = FVSdatabasepath & "\FramBaseCheck.Txt"

        If Exists(File_Name) Then
            'rssw.Close()
            Delete(File_Name)
        End If
        rssw = CreateText(File_Name)
        PrnLine = "Command File =" + FVSdatabasepath + "\" & "1" & "     " & Date.Now.ToString
        rssw.WriteLine(PrnLine)
        rssw.WriteLine(" ")

        Dim FramDB As New SQLite.SQLiteConnection("Data Source=" & FVSdatabasename & ";Version=3;Compress=True;")
        FramDB.Open()
        cmd1.Connection = FramDB

        '- Read BASE PERIOD Selection
        cmd1.CommandText = "SELECT * FROM BaseID WHERE BasePeriodID = " & BaseNum.ToString
        drd1 = cmd1.ExecuteReader
        FoundBaseID = 0
        Do While drd1.Read()
            FoundBaseID = 1
            Exit Do
        Loop
        If FoundBaseID = 0 Then
            '- Can't Find Base Period ID Record for this RunID (Deleted??)
            Result = MsgBox("Can't find the Base Period ID for this Model Run!" & vbCrLf & "Do you want to Choose another Base Period ???", MsgBoxStyle.YesNo)
            If Result = vbNo Then
                FramDB.Close()
                rssw.Close()
                Me.Cursor = Cursors.Default
                Exit Sub
            End If
            '- Choose Base Period for this "Orphan" RunID
            FramDB.Close()
            ModelRunBPSelect = True
            'FVS_BasePeriodSelect.ShowDialog()
            Me.BringToFront()
            If BasePeriodIDSelect = 0 Then
                FramDB.Close()
                rssw.Close()
                Exit Sub
            Else
                '- Change RunID Record to point at new Base Period Selection
                FramDB.Open()
                CmdStr = "SELECT * FROM RunID WHERE RunID = " & RunIDSelect.ToString & ";"
                Dim RIDcm As New SQLite.SQLiteCommand(CmdStr, FramDB)
                Dim RunIDDA As New System.Data.SQLite.SQLiteDataAdapter
                RunIDDA.SelectCommand = RIDcm
                Dim RIDcb As New SQLite.SQLiteCommandBuilder
                RIDcb = New SQLite.SQLiteCommandBuilder(RunIDDA)
                If FramDataSet.Tables.Contains("RunID") Then
                    FramDataSet.Tables("RunID").Clear()
                End If
                RunIDDA.Fill(FramDataSet, "RunID") '**************************************

                Dim NumRID As Integer
                NumRID = FramDataSet.Tables("RunID").Rows.Count
                If NumRID <> 1 Then
                    MsgBox("ERROR in RunID Table of Database ... Duplicate Record", MsgBoxStyle.OkOnly)
                End If
                FramDataSet.Tables("RunID").Rows(0)(5) = BasePeriodIDSelect
                RunIDDA.Update(FramDataSet, "RunID")
                RunIDDA = Nothing
                ' ReRead New Base Period Record Selection
                cmd1.CommandText = "SELECT * FROM BaseID WHERE BasePeriodID = " & BasePeriodIDSelect.ToString
                drd1 = cmd1.ExecuteReader
                drd1.Read()
            End If
        End If

        BasePeriodID = drd1.GetInt32(1)
        BasePeriodIDSelect = BasePeriodID
        BasePeriodName = drd1.GetString(2)
        SpeciesName = drd1.GetString(3)
        NumStk = drd1.GetInt32(4)
        NumFish = drd1.GetInt32(5)
        NumSteps = drd1.GetInt32(6)
        NumAge = drd1.GetInt32(7)
        MinAge = drd1.GetInt32(8)
        MaxAge = drd1.GetInt32(9)
        'BasePeriodDate = drd1.GetDateTime(10)
        BasePeriodComments = drd1.GetString(11)
        StockVersion = drd1.GetInt32(12)
        FisheryVersion = drd1.GetInt32(13)
        TimeStepVersion = drd1.GetInt32(14)
        'cmd1.Dispose()
        drd1.Dispose()

        '- Text File Printing
        Dim sb As New StringBuilder

        '- ReDim Base Arrays
        Call ReDimBaseArrays()

        '- ReDim Calculation and Input Arrays
        Call ReDimCalcArrays()

        '- Read RUN Selection Variables

        '*****************************************************************************************************
        Dim RunIDTable As String = "RunID"
        CmdStr = "SELECT * FROM [" & RunIDTable & "];"
        Dim RunID1cm As New SQLite.SQLiteCommand(CmdStr, FramDB)
        Dim RunID1DA As New System.Data.SQLite.SQLiteDataAdapter
        RunID1DA.SelectCommand = RunID1cm
        Dim RunID1cb As New SQLite.SQLiteCommandBuilder
        RunID1cb = New SQLite.SQLiteCommandBuilder(RunID1DA)
        RunID1DA.Fill(FramDataSet, "RunID")


        'Dim col As DataColumn

        Dim i As Integer = 1

        i = FramDataSet.Tables(RunIDTable).Columns.IndexOf("RunYear")
        If i = -1 Then 'This Column is missing so add it
            RunID1cm.CommandText = "ALTER TABLE " & RunIDTable & " ADD " & "RunYear" & " " & "String"
            RunID1cm.ExecuteNonQuery()   'executes the SQL code in cmd without querry
        End If


        i = FramDataSet.Tables(RunIDTable).Columns.IndexOf("RunType")
        If i = -1 Then 'This Column is missing so add it
            RunID1cm.CommandText = "ALTER TABLE " & RunIDTable & " ADD " & "RunType" & " " & "String"
            RunID1cm.ExecuteNonQuery()   'executes the SQL code in cmd without querry
        End If

        i = FramDataSet.Tables(RunIDTable).Columns.IndexOf("TAMMName")
        If i = -1 Then 'This Column is missing so add it
            RunID1cm.CommandText = "ALTER TABLE " & RunIDTable & " ADD " & "TAMMName" & " " & "String"
            RunID1cm.ExecuteNonQuery()   'executes the SQL code in cmd without querry
        End If

        i = FramDataSet.Tables(RunIDTable).Columns.IndexOf("CoastalIterations")
        If i = -1 Then 'This Column is missing so add it
            RunID1cm.CommandText = "ALTER TABLE " & RunIDTable & " ADD " & "CoastalIterations" & " " & "String"
            RunID1cm.ExecuteNonQuery()   'executes the SQL code in cmd without querry
        End If

        i = FramDataSet.Tables(RunIDTable).Columns.IndexOf("FRAMVersion")
        If i = -1 Then 'This Column is missing so add it
            RunID1cm.CommandText = "ALTER TABLE " & RunIDTable & " ADD " & "FRAMVersion" & " " & "String"
            RunID1cm.ExecuteNonQuery()   'executes the SQL code in cmd without querry
        End If



        '*****************************************************************

        cmd1.Connection = FramDB
        cmd1.CommandText = "SELECT * FROM RunID WHERE RunID = " & CStr(RunNum)
        drd1 = cmd1.ExecuteReader
        drd1.Read()

        RunIDSelect = drd1.GetInt32(1)          '- Current Run ID (User Selection)
        RunIDNameSelect = drd1.GetString(3)     '- Current Run Name
        RunIDTitleSelect = drd1.GetString(4)    '- Current Run Title
        RunIDCommentsSelect = drd1.GetString(6)   '- Current Run Comments
        'RunIDCreationDateSelect = drd1.GetDateTime(7)
        'RunIDModifyInputDateSelect = drd1.GetDateTime(8)
        'RunIDRunTimeDateSelect = drd1.GetDateTime(9)


        Try
            RunIDYearSelect = drd1.GetString(10)
        Catch Ex As Exception
            MsgBox("Please provide a run year in the RunID table of the AccessDB for RunID " & RunIDSelect & ". You can also enter the run year under FRAMUtilities/EditModelRunInfo.")
            RunIDYearSelect = 0
        End Try

        Try
            RunIDTypeSelect = drd1.GetString(11)
        Catch Ex As Exception
            'MsgBox("Please provide a Run Type (Pre or Post)in the RunID table of the AccessDB for RunType. " & RunIDSelect & ". You can also enter the run year under FRAMUtilities/EditModelRunInfo.")
            RunIDTypeSelect = ""
        End Try

        Try
            TAMMName = drd1.GetString(12)
        Catch Ex As Exception
            'MsgBox("Please provide a Run Type (Pre or Post)in the RunID table of the AccessDB for RunType. " & RunIDSelect & ". You can also enter the run year under FRAMUtilities/EditModelRunInfo.")
            TAMMName = "unknown"
        End Try

        Try
            CoastalIter = drd1.GetString(13)
        Catch Ex As Exception
            'MsgBox("Please provide a Run Type (Pre or Post)in the RunID table of the AccessDB for RunType. " & RunIDSelect & ". You can also enter the run year under FRAMUtilities/EditModelRunInfo.")
            CoastalIter = ""
        End Try

        Try
            FRAMVers = drd1.GetString(14)
        Catch Ex As Exception
            'MsgBox("Please provide a Run Type (Pre or Post)in the RunID table of the AccessDB for RunType. " & RunIDSelect & ". You can also enter the run year under FRAMUtilities/EditModelRunInfo.")
            FRAMVers = "unknown"
        End Try

        'cmd1.Dispose()
        drd1.Dispose()


        '- Read Base Period Cohort Size Data
        cmd1.Connection = FramDB
        cmd1.CommandText = "SELECT * FROM BaseCohort WHERE BasePeriodID = " & BasePeriodID.ToString
        drd1 = cmd1.ExecuteReader
        Dim BPID As Integer
        Do While drd1.Read
            BPID = drd1.GetInt32(0)
            Stk = drd1.GetInt32(1)
            Age = drd1.GetInt32(2)
            If Stk > NumStk Or Age > MaxAge Then
                MsgBox("ERROR in BaseCohort Table", MsgBoxStyle.OkOnly)
            End If
            BaseCohortSize(Stk, Age) = drd1.GetDouble(3)   '- Base Period Cohort Size
        Loop
        'cmd1.Dispose()
        drd1.Dispose()

        '- Read Base Period Exploitation Rate Data
        cmd1.Connection = FramDB
        cmd1.CommandText = "SELECT * FROM BaseExploitationRate WHERE BasePeriodID = " & BasePeriodID.ToString
        drd1 = cmd1.ExecuteReader
        Do While drd1.Read
            BPID = drd1.GetInt32(0)
            Stk = drd1.GetInt32(1)
            Age = drd1.GetInt32(2)
            Fish = drd1.GetInt32(3)
            TStep = drd1.GetInt32(4)
            If Stk > NumStk Or Age > MaxAge Or Fish > NumFish Or TStep > NumSteps Then
                MsgBox("ERROR in Base Exploitation Rate Table", MsgBoxStyle.OkOnly)
            End If
            BaseExploitationRate(Stk, Age, Fish, TStep) = drd1.GetDouble(5) '- BPER
            BaseSubLegalRate(Stk, Age, Fish, TStep) = drd1.GetDouble(6) '- BPER
            If BaseExploitationRate(Stk, Age, Fish, TStep) <> 0 Then
                AnyBaseRate(Fish, TStep) = 1
            End If
        Loop
        'cmd1.Dispose()
        drd1.Dispose()

        '- Read Base Maturation Rate Data
        cmd1.Connection = FramDB
        cmd1.CommandText = "SELECT * FROM MaturationRate WHERE BasePeriodID = " & BasePeriodID.ToString
        drd1 = cmd1.ExecuteReader
        Dim StkVers As Integer
        Do While drd1.Read
            BPID = drd1.GetInt32(0)
            Stk = drd1.GetInt32(1)
            Age = drd1.GetInt32(2)
            TStep = drd1.GetInt32(3)
            If Stk > NumStk Or Age > MaxAge Or TStep > NumSteps Then
                MsgBox("ERROR in Maturation Rate Table", MsgBoxStyle.OkOnly)
            End If
            MaturationRate(Stk, Age, TStep) = drd1.GetDouble(4) '- Maturation Rate
        Loop
        'cmd1.Dispose()
        drd1.Dispose()

        '- Read Base Stock Data
        cmd1.Connection = FramDB
        cmd1.CommandText = "SELECT * FROM Stock WHERE Species = " & Chr(34) & SpeciesName.ToString & Chr(34) & " AND StockVersion = " & StockVersion.ToString & " ORDER BY StockID"
        drd1 = cmd1.ExecuteReader
        Dim StkSpec As String
        Dim StkNum As Integer
        StkNum = 0
        Do While drd1.Read
            StkNum += 1
            StkSpec = drd1.GetString(0)
            StkVers = drd1.GetInt32(1)
            If StkSpec <> SpeciesName Or StkVers <> StockVersion Then
                MsgBox("Error in Stock Table Read", MsgBoxStyle.OkOnly)
            End If
            StockID(StkNum) = drd1.GetInt32(2)              '- Stock Number
            ProductionRegion(StkNum) = drd1.GetInt32(3)     '- PR Number
            ManagementUnit(StkNum) = drd1.GetInt32(4)       '- MU Number
            StockName(StkNum) = drd1.GetString(5)           '- Stock Short Name
            StockTitle(StkNum) = drd1.GetString(6)          '- Stock Long Name
        Loop
        If StkNum <> NumStk Then
            If ImportStock = False Then
                MsgBox("Error in Stock Table Read - Bad Record count", MsgBoxStyle.OkOnly)
            End If
        End If
        'cmd1.Dispose()
        drd1.Dispose()

        '- Read Natural Mortality Rates (Base Data)
        cmd1.Connection = FramDB
        cmd1.CommandText = "SELECT * FROM NaturalMortality WHERE BasePeriodID = " & BasePeriodID.ToString
        drd1 = cmd1.ExecuteReader
        Do While drd1.Read
            BPID = drd1.GetInt32(0)
            Age = drd1.GetInt32(1)
            TStep = drd1.GetInt32(2)
            If Age > MaxAge Or TStep > NumSteps Then
                MsgBox("Error in NaturalMortality Table Read", MsgBoxStyle.OkOnly)
            End If
            NaturalMortality(Age, TStep) = drd1.GetDouble(3) '- Natural Mortality Rate
        Loop
        'cmd1.Dispose()
        drd1.Dispose()

        '- Temp Fix for BaseERate, Nat Mort Rate, and Maturity Rate Time 4
        '- time 4 Chinook are the same age as time 1 Chinook of the same age (see Popstat)
        '- The calibration program mistakenly sets time 4 Chinook ER to time 1 Chinook ER of age +1
        '- this code sets it back
        '- Error in Base Period write routine for Time 4
        If SpeciesName = "CHINOOK" Then
            For Stk As Integer = 1 To NumStk
                For Age As Integer = 2 To MaxAge
                    For Fish As Integer = 1 To NumFish
                        BaseExploitationRate(Stk, Age, Fish, 4) = BaseExploitationRate(Stk, Age, Fish, 1)
                        BaseSubLegalRate(Stk, Age, Fish, 4) = BaseSubLegalRate(Stk, Age, Fish, 1)
                    Next Fish%
                    MaturationRate(Stk, Age, 4) = MaturationRate(Stk, Age, 1)
                    NaturalMortality(Age, 4) = NaturalMortality(Age, 1)
                Next
            Next
        End If

        '- Read Base Fishery Data
        cmd1.Connection = FramDB
        cmd1.CommandText = "SELECT * FROM Fishery WHERE Species = " & Chr(34) & SpeciesName.ToString & Chr(34) & " AND VersionNumber = " & FisheryVersion.ToString & " ORDER BY FisheryID"
        drd1 = cmd1.ExecuteReader
        Dim FishSpec As String
        Dim FishVers, FishNum As Integer
        FishNum = 0
        Do While drd1.Read
            FishNum += 1
            FishSpec = drd1.GetString(0)
            FishVers = drd1.GetInt32(1)
            If FishSpec <> SpeciesName Or FishVers <> FisheryVersion Then
                MsgBox("Error in Fishery Table Read", MsgBoxStyle.OkOnly)
            End If
            FisheryID(FishNum) = drd1.GetInt32(2)      '- Fishery Number
            FisheryName(FishNum) = drd1.GetString(3)   '- Fishery Short Name
            FisheryTitle(FishNum) = drd1.GetString(4)  '- Fishery Long Name
        Loop
        If FishNum <> NumFish Then
            MsgBox("Error in Fishery Table Read - Bad Record count", MsgBoxStyle.OkOnly)
        End If
        'cmd1.Dispose()
        drd1.Dispose()

        '- Read Base Time Step Data
        cmd1.Connection = FramDB
        cmd1.CommandText = "SELECT * FROM TimeStep WHERE Species = " & Chr(34) & SpeciesName.ToString & Chr(34) & " AND VersionNumber = " & TimeStepVersion.ToString & " ORDER BY TimeStepID"
        drd1 = cmd1.ExecuteReader
        Dim TimeSpec As String
        Dim TimeVers, TimeNum As Integer
        TimeNum = 0
        Do While drd1.Read
            TimeNum += 1
            TimeSpec = drd1.GetString(0)
            TimeVers = drd1.GetInt32(1)
            If TimeSpec <> SpeciesName Or TimeVers <> TimeStepVersion Then
                MsgBox("Error in Time Step Table Read", MsgBoxStyle.OkOnly)
            End If
            TimeStepID(TimeNum) = drd1.GetInt32(2)      '- Time Step Number
            TimeStepName(TimeNum) = drd1.GetString(3)   '- Time Step Short Name
            TimeStepTitle(TimeNum) = drd1.GetString(4)  '- Time Step Long Name
        Loop
        If TimeNum <> NumSteps Then
            MsgBox("Error in Time Step Table Read - Bad Record count", MsgBoxStyle.OkOnly)
        End If
        'cmd1.Dispose()
        drd1.Dispose()

        '- Read Fishery Incidental Mortality Rates (Base Data)
        cmd1.Connection = FramDB
        cmd1.CommandText = "SELECT * FROM IncidentalRate WHERE BasePeriodID = " & BasePeriodID.ToString
        drd1 = cmd1.ExecuteReader
        Do While drd1.Read
            RunIDNum = drd1.GetInt32(0)
            Fish = drd1.GetInt32(1)
            TStep = drd1.GetInt32(2)
            If Fish > NumFish Or TStep > NumSteps Then
                MsgBox("Error in FisherScalers Table Read", MsgBoxStyle.OkOnly)
            End If
            IncidentalRate(Fish, TStep) = drd1.GetDouble(3) '- Incidental Mortality Rate
        Loop
        'cmd1.Dispose()
        drd1.Dispose()

        '- Read Fishery Shaker Release Mortality Rates (Base Data)
        cmd1.Connection = FramDB
        cmd1.CommandText = "SELECT * FROM ShakerMortRate WHERE BasePeriodID = " & BasePeriodID.ToString
        drd1 = cmd1.ExecuteReader
        Do While drd1.Read
            RunIDNum = drd1.GetInt32(0)
            Fish = drd1.GetInt32(1)
            TStep = drd1.GetInt32(2)
            If Fish > NumFish Or TStep > NumSteps Then
                MsgBox("Error in FisherScalers Table Read", MsgBoxStyle.OkOnly)
            End If
            ShakerMortRate(Fish, TStep) = drd1.GetDouble(3) '- Incidental Mortality Rate
        Loop
        'cmd1.Dispose()
        drd1.Dispose()

        '- Read Terminal Fishery Flags (Base Data by Time Step)
        cmd1.Connection = FramDB
        cmd1.CommandText = "SELECT * FROM TerminalFisheryFlag WHERE BasePeriodID = " & BasePeriodID.ToString
        drd1 = cmd1.ExecuteReader
        Do While drd1.Read
            RunIDNum = drd1.GetInt32(0)
            Fish = drd1.GetInt32(1)
            TStep = drd1.GetInt32(2)
            If Fish > NumFish Or TStep > NumSteps Then
                MsgBox("Error in Terminal Fishery Flag Table Read", MsgBoxStyle.OkOnly)
            End If
            TerminalFisheryFlag(Fish, TStep) = drd1.GetInt32(3) '- Terminal Fishery Flag
        Loop
        'cmd1.Dispose()
        drd1.Dispose()

        '- Read Base Fishery Model Stock Proportion (Proportion Model Stocks of Entire Fishery Catch)
        cmd1.Connection = FramDB
        cmd1.CommandText = "SELECT * FROM FisheryModelStockProportion WHERE BasePeriodID = " & BasePeriodID.ToString
        drd1 = cmd1.ExecuteReader
        Do While drd1.Read
            RunIDNum = drd1.GetInt32(0)
            Fish = drd1.GetInt32(1)
            If Fish > NumFish Then
                MsgBox("Error in Model Stock Proportion Table Read", MsgBoxStyle.OkOnly)
            End If
            ModelStockProportion(Fish) = drd1.GetDouble(2) '- Model Stock Proportion of Fishery/Time-Step
        Loop
        'cmd1.Dispose()
        drd1.Dispose()

        '- Read Base Von Bertlanffy Growth Parameters for CHINOOK
        If SpeciesName = "CHINOOK" Then
            If NumStk < 78 Then 'use old growth function mid-points
                MidTimeStep(1) = 1
                MidTimeStep(2) = 5.5
                MidTimeStep(3) = 8
                MidTimeStep(4) = 1
            Else
                'updated for Pete's new growth functions
                MidTimeStep(1) = 3.5
                MidTimeStep(2) = 8
                MidTimeStep(3) = 10.5
                MidTimeStep(4) = 3.5
            End If


            cmd1.Connection = FramDB
            cmd1.CommandText = "SELECT * FROM Growth WHERE BasePeriodID = " & BasePeriodID.ToString
            drd1 = cmd1.ExecuteReader
            Do While drd1.Read
                RunIDNum = drd1.GetInt32(0)
                Stk = drd1.GetInt32(1)
                If Stk > NumStk Then
                    MsgBox("Error in Model Stock Proportion Table Read", MsgBoxStyle.OkOnly)
                End If
                VonBertL(Stk, 0) = drd1.GetDouble(2)
                VonBertK(Stk, 0) = drd1.GetDouble(3)
                VonBertT(Stk, 0) = drd1.GetDouble(4)
                VonBertCV(Stk, 2, 0) = drd1.GetDouble(5)
                VonBertCV(Stk, 3, 0) = drd1.GetDouble(6)
                VonBertCV(Stk, 4, 0) = drd1.GetDouble(7)
                VonBertCV(Stk, 5, 0) = drd1.GetDouble(8)
                VonBertL(Stk, 1) = drd1.GetDouble(9)
                VonBertK(Stk, 1) = drd1.GetDouble(10)
                VonBertT(Stk, 1) = drd1.GetDouble(11)
                VonBertCV(Stk, 2, 1) = drd1.GetDouble(12)
                VonBertCV(Stk, 3, 1) = drd1.GetDouble(13)
                VonBertCV(Stk, 4, 1) = drd1.GetDouble(14)
                VonBertCV(Stk, 5, 1) = drd1.GetDouble(15)
                If Stk > NumStk Then
                    MsgBox("Error in Model Stock Proportion Table Read", MsgBoxStyle.OkOnly)
                End If
            Loop
            'cmd1.Dispose()
            drd1.Dispose()
        ElseIf SpeciesName = "COHO" Then
            '- Place Holder Values for COHO
            For Stk As Integer = 1 To NumStk
                For Age As Integer = MinAge To MaxAge
                    For TStep As Integer = 1 To NumSteps
                        AEQ(Stk, Age, TStep) = 1.0
                    Next TStep
                Next Age
            Next Stk
            For Stk As Integer = 1 To NumStk
                For TermStat As Integer = PTerm To Term
                    VonBertL(Stk, TermStat) = 100.0
                    VonBertT(Stk, TermStat) = 1.0
                    VonBertK(Stk, TermStat) = 0.03
                    For Age As Integer = MinAge To MaxAge
                        VonBertCV(Stk, Age, TermStat) = 0.1
                    Next Age
                Next TermStat
            Next Stk
            MidTimeStep(1) = 3.5
            MidTimeStep(2) = 7.5
            MidTimeStep(3) = 8.5
            MidTimeStep(4) = 9.5
            'MidTimeStep(5) = 11




        End If
        'cmd1.Dispose()
        drd1.Dispose()

        If SpeciesName = "CHINOOK" Then
            '- Read Base AEQ Rate Data (Adult Equivalent Rate)
            cmd1.Connection = FramDB
            cmd1.CommandText = "SELECT * FROM AEQ WHERE BasePeriodID = " & BasePeriodID.ToString
            drd1 = cmd1.ExecuteReader
            Do While drd1.Read
                BPID = drd1.GetInt32(0)
                Stk = drd1.GetInt32(1)
                Age = drd1.GetInt32(2)
                TStep = drd1.GetInt32(3)
                If Stk > NumStk Or Age > MaxAge Or TStep > NumSteps Then
                    MsgBox("ERROR in AEQ Rate Table", MsgBoxStyle.OkOnly)
                End If
                AEQ(Stk, Age, TStep) = drd1.GetDouble(4) '- AEQ Value
            Loop
            'cmd1.Dispose()
            drd1.Dispose()

            '- Read Base EncounterRateAdjustment Data (Shaker Encounter Adjustment)
            cmd1.Connection = FramDB
            cmd1.CommandText = "SELECT * FROM EncounterRateAdjustment WHERE BasePeriodID = " & BasePeriodID.ToString
            drd1 = cmd1.ExecuteReader
            Do While drd1.Read
                BPID = drd1.GetInt32(0)
                Age = drd1.GetInt32(1)
                Fish = drd1.GetInt32(2)
                TStep = drd1.GetInt32(3)
                If Fish > NumFish Or Age > MaxAge Or TStep > NumSteps Then
                    MsgBox("ERROR in EncounterRateAdjustment Table", MsgBoxStyle.OkOnly)
                End If
                EncounterRateAdjustment(Age, Fish, TStep) = drd1.GetDouble(4) '- AEQ Value
            Loop
            'cmd1.Dispose()
            drd1.Dispose()

            '- Read Chinook Base Calibration EncounterRateAdjustment Data by Fishery, TimeStep
            cmd1.Connection = FramDB
            cmd1.CommandText = "SELECT * FROM ChinookBaseEncounterAdjustment"
            drd1 = cmd1.ExecuteReader
            Do While drd1.Read
                Fish = drd1.GetInt32(0)
                If Fish > NumFish Then
                    MsgBox("ERROR in ChinookBaseEncounterAdjustment Table", MsgBoxStyle.OkOnly)
                End If
                ChinookBaseEncounterAdjustment(Fish, 1) = drd1.GetDouble(1)
                ChinookBaseEncounterAdjustment(Fish, 2) = drd1.GetDouble(2)
                ChinookBaseEncounterAdjustment(Fish, 3) = drd1.GetDouble(3)
                ChinookBaseEncounterAdjustment(Fish, 4) = drd1.GetDouble(4)
            Loop
            'cmd1.Dispose()
            drd1.Dispose()

            '- Read Chinook Base Calibration Size Limit Data by Fishery, TimeStep
            'cmd1.Connection = FramDB


            CmdStr = "SELECT * FROM ChinookBaseSizeLimit"
            Dim BSLcm As New SQLite.SQLiteCommand(CmdStr, FramDB)
            Dim BSLDA As New System.Data.SQLite.SQLiteDataAdapter
            BSLDA.SelectCommand = BSLcm
            Dim BSLcb As New SQLite.SQLiteCommandBuilder
            BSLcb = New SQLite.SQLiteCommandBuilder(BSLDA)

            'FramDataSet.Clear()
            BSLDA.Fill(FramDataSet, "ChinookBaseSizeLimit")
            BSLDA.Update(FramDataSet, "ChinookBaseSizeLimit")
            ' FramDataSet.Tables("ChinookBaseSizeLimit").Clear()
            'BSLDA.Fill(FramDataSet, "ChinookBaseSizeLimit")

            If FramDataSet.Tables.Contains("ChinookBaseSizeLimit") Then
                FramDataSet.Tables.Remove("ChinookBaseSizeLimit")
            End If
            BSLDA.Fill(FramDataSet, "ChinookBaseSizeLimit")

            i = FramDataSet.Tables("ChinookBaseSizeLimit").Columns.IndexOf("BasePeriodID")
            If i = -1 Then 'This Column is missing 
                BPSL_No_ID = True
            Else
                BPSL_No_ID = False
            End If
            Dim k As Integer
            k = FramDataSet.Tables("ChinookBaseSizeLimit").Rows.Count
            For RecNum = 0 To k - 1
                If BPSL_No_ID = False Then 'table has BaseID field       
                    BPID = FramDataSet.Tables("ChinookBaseSizeLimit").Rows(RecNum)(0)
                    If BPID = BasePeriodIDSelect Then
                        Fish = FramDataSet.Tables("ChinookBaseSizeLimit").Rows(RecNum)(1)
                        If Fish > NumFish Then
                            MsgBox("ERROR in ChinookBaseSizeLimit Table", MsgBoxStyle.OkOnly)
                        End If
                        ChinookBaseSizeLimit(Fish, 1) = FramDataSet.Tables("ChinookBaseSizeLimit").Rows(RecNum)(2)
                        ChinookBaseSizeLimit(Fish, 2) = FramDataSet.Tables("ChinookBaseSizeLimit").Rows(RecNum)(3)
                        ChinookBaseSizeLimit(Fish, 3) = FramDataSet.Tables("ChinookBaseSizeLimit").Rows(RecNum)(4)
                        ChinookBaseSizeLimit(Fish, 4) = FramDataSet.Tables("ChinookBaseSizeLimit").Rows(RecNum)(5)
                    Else
                        Jim = 1
                    End If
                Else 'table does not have BaseID field
                    Fish = FramDataSet.Tables("ChinookBaseSizeLimit").Rows(RecNum)(0)
                    If Fish > NumFish Then
                        MsgBox("ERROR in ChinookBaseSizeLimit Table", MsgBoxStyle.OkOnly)
                    End If
                    ChinookBaseSizeLimit(Fish, 1) = FramDataSet.Tables("ChinookBaseSizeLimit").Rows(RecNum)(1)
                    ChinookBaseSizeLimit(Fish, 2) = FramDataSet.Tables("ChinookBaseSizeLimit").Rows(RecNum)(2)
                    ChinookBaseSizeLimit(Fish, 3) = FramDataSet.Tables("ChinookBaseSizeLimit").Rows(RecNum)(3)
                    ChinookBaseSizeLimit(Fish, 4) = FramDataSet.Tables("ChinookBaseSizeLimit").Rows(RecNum)(4)
                End If
            Next
        End If

        '------- Finished with Base Data Reads for Populating Arrays --------------

        '- Fill the FramDataSet Object with Tables for the Data that will Change
        '- Use OleDbDataAdapter Method instead of OleDb Command

        CmdStr = "SELECT * FROM FisheryScalers WHERE RunID = " & RunIDSelect.ToString & " ORDER BY FisheryID, TimeStep"
        Dim FScm As New SQLite.SQLiteCommand(CmdStr, FramDB)
        Dim FishScalerDA As New System.Data.SQLite.SQLiteDataAdapter
        FishScalerDA.SelectCommand = FScm
        Dim FScb As New SQLite.SQLiteCommandBuilder
        FScb = New SQLite.SQLiteCommandBuilder(FishScalerDA)
        FishScalerDA.Fill(FramDataSet, "FisheryScalers")
        Dim NumFS As Integer
        NumFS = FramDataSet.Tables("FisheryScalers").Rows.Count

        If FramDataSet.Tables.Contains("FisheryScalers") Then
            FramDataSet.Tables("FisheryScalers").Clear()
        End If
        'Dim table As DataTable
        Dim column As DataColumn

        ' For each DataTable, print the ColumnName.
        'For Each table In DataSet.Tables
        For Each column In FramDataSet.Tables("FisheryScalers").Columns
            If (column.ColumnName) = "MSFFisheryScaleFactor" Then GoTo FoundNewColumn
        Next
        'Next
        MsgBox("Wrong Format for Database Table 'FisheryScalers' !!!!" & vbCrLf & "You have the WRONG Type database (ie Old Version VS)" & vbCrLf &
               "Please Choose Another Database to use" & vbCrLf & "with this Version of FramVS (Multiple MSF)", MsgBoxStyle.OkOnly)
        End
FoundNewColumn:
        FramDataSet.Tables("FisheryScalers").Clear()
        FishScalerDA.Fill(FramDataSet, "FisheryScalers")
        NumFS = FramDataSet.Tables("FisheryScalers").Rows.Count
        For RecNum = 0 To NumFS - 1
            RunIDNum = FramDataSet.Tables("FisheryScalers").Rows(RecNum)(1)
            Fish = FramDataSet.Tables("FisheryScalers").Rows(RecNum)(2)
            TStep = FramDataSet.Tables("FisheryScalers").Rows(RecNum)(3)
            If Fish > NumFish Or TStep > NumSteps Then
                MsgBox("Error in FisherScalers Table Read", MsgBoxStyle.OkOnly)
            End If
            FisheryFlag(Fish, TStep) = FramDataSet.Tables("FisheryScalers").Rows(RecNum)(4)
            FisheryScaler(Fish, TStep) = FramDataSet.Tables("FisheryScalers").Rows(RecNum)(5)
            FisheryQuota(Fish, TStep) = FramDataSet.Tables("FisheryScalers").Rows(RecNum)(6)
            If IsDBNull(FramDataSet.Tables("FisheryScalers").Rows(RecNum)(7)) Then
                If FisheryFlag(Fish, TStep) = 7 Or FisheryFlag(Fish, TStep) = 8 Then
                    MSFFisheryScaler(Fish, TStep) = FisheryScaler(Fish, TStep)
                    FisheryScaler(Fish, TStep) = 0
                Else
                    MSFFisheryScaler(Fish, TStep) = 0
                End If
            Else
                MSFFisheryScaler(Fish, TStep) = FramDataSet.Tables("FisheryScalers").Rows(RecNum)(7)
            End If
            If IsDBNull(FramDataSet.Tables("FisheryScalers").Rows(RecNum)(8)) Then
                If FisheryFlag(Fish, TStep) = 7 Or FisheryFlag(Fish, TStep) = 8 Then
                    MSFFisheryQuota(Fish, TStep) = FisheryQuota(Fish, TStep)
                    FisheryQuota(Fish, TStep) = 0
                Else
                    MSFFisheryQuota(Fish, TStep) = 0
                End If
            Else
                MSFFisheryQuota(Fish, TStep) = FramDataSet.Tables("FisheryScalers").Rows(RecNum)(8)
            End If
            MarkSelectiveMortRate(Fish, TStep) = FramDataSet.Tables("FisheryScalers").Rows(RecNum)(9)
            MarkSelectiveMarkMisID(Fish, TStep) = FramDataSet.Tables("FisheryScalers").Rows(RecNum)(10)
            MarkSelectiveUnMarkMisID(Fish, TStep) = FramDataSet.Tables("FisheryScalers").Rows(RecNum)(11)
            MarkSelectiveIncRate(Fish, TStep) = FramDataSet.Tables("FisheryScalers").Rows(RecNum)(12)
            If FramDataSet.Tables("FisheryScalers").Columns.IndexOf("Comment") <> -1 Then
                FisheryComment(Fish, TStep) = FramDataSet.Tables("FisheryScalers").Rows(RecNum)(13)
            End If
        Next

        '- Read Stock Recruit Input Scalers Data
        CmdStr = "SELECT * FROM StockRecruit WHERE RunID = " & RunIDSelect.ToString & " ORDER BY StockID, Age"
        Dim SRcm As New SQLite.SQLiteCommand(CmdStr, FramDB)
        'Dim StockRecruitDA As New System.Data.SQLite.SQLiteDataAdapter
        StockRecruitDA.SelectCommand = SRcm
        Dim SRcb As New SQLite.SQLiteCommandBuilder
        SRcb = New SQLite.SQLiteCommandBuilder(StockRecruitDA)
        If FramDataSet.Tables.Contains("StockRecruit") Then
            FramDataSet.Tables("StockRecruit").Clear()
        End If
        StockRecruitDA.Fill(FramDataSet, "StockRecruit")
        Dim NumSR As Integer
        Dim SRCohort As Double
        NumSR = FramDataSet.Tables("StockRecruit").Rows.Count
        For RecNum = 0 To NumSR - 1
            RunIDNum = FramDataSet.Tables("StockRecruit").Rows(RecNum)(1)
            Stk = FramDataSet.Tables("StockRecruit").Rows(RecNum)(2)
            Age = FramDataSet.Tables("StockRecruit").Rows(RecNum)(3)
            If Stk > NumStk Or Age > MaxAge Then
                MsgBox("Error in Stock Recruit Table Read", MsgBoxStyle.OkOnly)
            End If
            StockRecruit(Stk, Age, 1) = FramDataSet.Tables("StockRecruit").Rows(RecNum)(4)
            StockRecruit(Stk, Age, 2) = FramDataSet.Tables("StockRecruit").Rows(RecNum)(5)
            '- Set Forecasted Stock Cohort Size using Recruit Scaler if not yet done
            SRCohort = StockRecruit(Stk, Age, 1) * BaseCohortSize(Stk, Age)
            If SRCohort <> StockRecruit(Stk, Age, 2) Then
                StockRecruit(Stk, Age, 2) = SRCohort
            End If
        Next


        '- Read NonRetention Flag and Input Data
        CmdStr = "SELECT * FROM NonRetention WHERE RunID = " & RunIDSelect.ToString & " ORDER BY FisheryID, TimeStep"
        Dim NRcm As New SQLite.SQLiteCommand(CmdStr, FramDB)
        Dim NonRetentionDA As New System.Data.SQLite.SQLiteDataAdapter
        '- Explicitly Zero Arrays
        For Fish As Integer = 0 To NumFish
            For TStep As Integer = 0 To NumSteps
                NonRetentionFlag(Fish, TStep) = 0
                For Age As Integer = 0 To 4
                    NonRetentionInput(Fish, TStep, Age) = 0
                Next
            Next
        Next
        NonRetentionDA.SelectCommand = NRcm
        Dim NRcb As New SQLite.SQLiteCommandBuilder
        NRcb = New SQLite.SQLiteCommandBuilder(NonRetentionDA)
        If FramDataSet.Tables.Contains("NonRetention") Then
            FramDataSet.Tables("NonRetention").Clear()
        End If
        NonRetentionDA.Fill(FramDataSet, "NonRetention")
        Dim NumNR As Integer
        NumNR = FramDataSet.Tables("NonRetention").Rows.Count
        '- Loop through Table Records for Actual Input Values
        For RecNum = 0 To NumNR - 1
            RunIDNum = FramDataSet.Tables("NonRetention").Rows(RecNum)(1)
            Fish = FramDataSet.Tables("NonRetention").Rows(RecNum)(2)
            TStep = FramDataSet.Tables("NonRetention").Rows(RecNum)(3)
            If Fish > NumFish Or TStep > NumSteps Then
                MsgBox("Error in NonRetention Table Read", MsgBoxStyle.OkOnly)
            End If
            NonRetentionFlag(Fish, TStep) = FramDataSet.Tables("NonRetention").Rows(RecNum)(4)
            NonRetentionInput(Fish, TStep, 1) = FramDataSet.Tables("NonRetention").Rows(RecNum)(5)
            If FramDataSet.Tables("NonRetention").Columns.IndexOf("Comment") <> -1 Then
                NonRetentionComment(Fish, TStep) = FramDataSet.Tables("NonRetention").Rows(RecNum)(9)
            End If
            If SpeciesName = "CHINOOK" Then
                NonRetentionInput(Fish, TStep, 2) = FramDataSet.Tables("NonRetention").Rows(RecNum)(6)
                NonRetentionInput(Fish, TStep, 3) = FramDataSet.Tables("NonRetention").Rows(RecNum)(7)
                NonRetentionInput(Fish, TStep, 4) = FramDataSet.Tables("NonRetention").Rows(RecNum)(8)
            End If
        Next

        '- Read Size Limit Input Data
        CmdStr = "SELECT * FROM SizeLimits WHERE RunID = " & RunIDSelect.ToString & " ORDER BY FisheryID, TimeStep"
        Dim SLcm As New SQLite.SQLiteCommand(CmdStr, FramDB)
        Dim SizeLimitsDA As New System.Data.SQLite.SQLiteDataAdapter
        SizeLimitsDA.SelectCommand = SLcm
        Dim SLcb As New SQLite.SQLiteCommandBuilder
        '- Explicitly Zero Arrays
        For Fish As Integer = 0 To NumFish
            For TStep As Integer = 0 To NumSteps
                MinSizeLimit(Fish, TStep) = 0
                MaxSizeLimit(Fish, TStep) = 0
            Next
        Next
        SLcb = New SQLite.SQLiteCommandBuilder(SizeLimitsDA)
        If FramDataSet.Tables.Contains("SizeLimits") Then
            FramDataSet.Tables("SizeLimits").Clear()
        End If
        SizeLimitsDA.Fill(FramDataSet, "SizeLimits")
        Dim NumSL As Integer
        NumSL = FramDataSet.Tables("SizeLimits").Rows.Count
        '- Loop through Table Records for Actual Input Values for CHINOOK
        If SpeciesName = "CHINOOK" Then
            For RecNum = 0 To NumSL - 1
                RunIDNum = FramDataSet.Tables("SizeLimits").Rows(RecNum)(1)
                Fish = FramDataSet.Tables("SizeLimits").Rows(RecNum)(2)
                TStep = FramDataSet.Tables("SizeLimits").Rows(RecNum)(3)
                If Fish > NumFish Or TStep > NumSteps Then
                    MsgBox("Error in SizeLimits Table Read", MsgBoxStyle.OkOnly)
                End If
                MinSizeLimit(Fish, TStep) = FramDataSet.Tables("SizeLimits").Rows(RecNum)(4)
                MaxSizeLimit(Fish, TStep) = FramDataSet.Tables("SizeLimits").Rows(RecNum)(5)
            Next
        ElseIf SpeciesName = "COHO" Then
            For Fish As Integer = 0 To NumFish
                For TStep As Integer = 0 To NumSteps
                    MinSizeLimit(Fish, TStep) = 10
                    MaxSizeLimit(Fish, TStep) = 1000
                Next
            Next
        End If

        '=================================================================================
        '- Pete 12/13 - Read in External Sublegal Variables
        '- Read Sublegal:Legal Ratio Data
        '- Don't bother with this code for coho

        If SpeciesName = "CHINOOK" Then

            CmdStr = "SELECT * FROM SLRatio WHERE RunID = " & RunIDSelect.ToString & " ORDER BY FisheryID, Age, TimeStep"

            Dim SLRatcm As New SQLite.SQLiteCommand(CmdStr, FramDB)
            Dim SLRatDA As New System.Data.SQLite.SQLiteDataAdapter
            SLRatDA.SelectCommand = SLRatcm
            Dim SLRatcb As New SQLite.SQLiteCommandBuilder
            SLRatcb = New SQLite.SQLiteCommandBuilder(SLRatDA)
            If FramDataSet.Tables.Contains("SLRatio") Then
                FramDataSet.Tables("SLRatio").Clear()
            End If
            SLRatDA.Fill(FramDataSet, "SLRatio")
            Dim NumSLRat As Integer
            NumSLRat = FramDataSet.Tables("SLRatio").Rows.Count

            '- Set target ratios and run encounter rate adjustments to 1.00 as default
            '  Note, these are zero-indexed arrays, so there's some slop in here (0s) in
            '  in the unused values (i.e., no fish, age, timestep match)
            For Fish As Integer = 1 To NumFish
                For Age As Integer = 2 To MaxAge
                    For TStep As Integer = 1 To NumSteps
                        'Set TargetRatio to -1 for if/then statement in ExternalSubCalcs subroutine [in FVS_RunModel.vb] to filter who gets updated
                        TargetRatio(Fish, Age, TStep) = -1
                        RunEncounterRateAdjustment(Fish, Age, TStep) = 1
                        UpdWhen(Fish, Age, TStep) = DateTime.Now 'Dummy filler
                        UpdBy(Fish, Age, TStep) = "not updated--ignore datetime" 'Dummy filler
                    Next
                Next
            Next

            '- Loop through Table Records for Actual Input Values
            For RecNum = 0 To NumSLRat - 1
                RunIDNum = FramDataSet.Tables("SLRatio").Rows(RecNum)(0)
                Fish = FramDataSet.Tables("SLRatio").Rows(RecNum)(1)
                Age = FramDataSet.Tables("SLRatio").Rows(RecNum)(2)
                TStep = FramDataSet.Tables("SLRatio").Rows(RecNum)(3)
                TargetRatio(Fish, Age, TStep) = FramDataSet.Tables("SLRatio").Rows(RecNum)(4)
                RunEncounterRateAdjustment(Fish, Age, TStep) = FramDataSet.Tables("SLRatio").Rows(RecNum)(5)
                UpdWhen(Fish, Age, TStep) = FramDataSet.Tables("SLRatio").Rows(RecNum)(6)
                UpdBy(Fish, Age, TStep) = FramDataSet.Tables("SLRatio").Rows(RecNum)(7)
            Next


            '- Now, replace EncounterRateAdjustment (i.e., ERA = ERA*RunERA)
            For Fish As Integer = 1 To NumFish
                For Age As Integer = 2 To MaxAge
                    For TStep As Integer = 1 To NumSteps
                        'For all intents and purposes, EncounterRateAdjustment for CHINOOK FRAM will now be EncounterRateAdjustment*RunEncounterRateAdjustment
                        'The default, as usual will still be 1.00 (no adjustment) and historical values will be retained at whatever they were too
                        EncounterRateAdjustment(Age, Fish, TStep) = EncounterRateAdjustment(Age, Fish, TStep) * RunEncounterRateAdjustment(Fish, Age, TStep)
                        'Kfat(Fish, Age, TStep) = 1
                    Next
                Next
            Next

        End If
        '=================================================================================


        '- Read StockFisheryRateScaler Data
        CmdStr = "SELECT * FROM StockFisheryRateScaler WHERE RunID = " & RunIDSelect.ToString & " ORDER BY StockID, FisheryID, TimeStep"
        Dim SFRcm As New SQLite.SQLiteCommand(CmdStr, FramDB)
        Dim StockFisheryDA As New System.Data.SQLite.SQLiteDataAdapter
        '- Set All StockFishRateScalers to ONE (Default Value)
        For Stk As Integer = 1 To NumStk
            For Fish As Integer = 1 To NumFish
                For TStep As Integer = 1 To NumSteps
                    StockFishRateScalers(Stk, Fish, TStep) = 1
                Next
            Next
        Next
        StockFisheryDA.SelectCommand = SFRcm
        Dim SFRcb As New SQLite.SQLiteCommandBuilder
        SFRcb = New SQLite.SQLiteCommandBuilder(StockFisheryDA)
        If FramDataSet.Tables.Contains("StockFisheryScaler") Then
            FramDataSet.Tables("StockFisheryScaler").Clear()
        End If
        StockFisheryDA.Fill(FramDataSet, "StockFisheryScaler")
        Dim NumSFR As Integer
        NumSFR = FramDataSet.Tables("StockFisheryScaler").Rows.Count
        '- Loop through Table Records for Actual Input Values
        For RecNum = 0 To NumSFR - 1
            RunIDNum = FramDataSet.Tables("StockFisheryScaler").Rows(RecNum)(0)
            Stk = FramDataSet.Tables("StockFisheryScaler").Rows(RecNum)(1)
            Fish = FramDataSet.Tables("StockFisheryScaler").Rows(RecNum)(2)
            TStep = FramDataSet.Tables("StockFisheryScaler").Rows(RecNum)(3)
            If Fish > NumFish Or TStep > NumSteps Then
                MsgBox("Error in StockFisheryRateScaler Table Read", MsgBoxStyle.OkOnly)
            End If
            StockFishRateScalers(Stk, Fish, TStep) = FramDataSet.Tables("StockFisheryScaler").Rows(RecNum)(4)
        Next
        StockFisheryDA = Nothing

        '- Read Backwards FRAM Target Escapement Data
        CmdStr = "SELECT * FROM BackwardsFRAM WHERE RunID = " & RunIDSelect.ToString
        Dim BFcm As New SQLite.SQLiteCommand(CmdStr, FramDB)
        Dim BFDA As New System.Data.SQLite.SQLiteDataAdapter
        BFDA.SelectCommand = BFcm
        Dim BFcb As New SQLite.SQLiteCommandBuilder
        BFcb = New SQLite.SQLiteCommandBuilder(BFDA)
        If FramDataSet.Tables.Contains("BackwardsFRAM") Then
            FramDataSet.Tables("BackwardsFRAM").Clear()
        End If
        BFDA.Fill(FramDataSet, "BackwardsFRAM")
        Dim NumBF As Integer
        NumBF = FramDataSet.Tables("BackwardsFRAM").Rows.Count
        '- Loop through Table Records for Actual Values
        For RecNum = 0 To NumBF - 1
            RunIDNum = FramDataSet.Tables("BackwardsFRAM").Rows(RecNum)(0)
            Stk = FramDataSet.Tables("BackwardsFRAM").Rows(RecNum)(1)
            If SpeciesName = "COHO" Then
                BackwardsTarget(Stk) = FramDataSet.Tables("BackwardsFRAM").Rows(RecNum)(2)
            ElseIf SpeciesName = "CHINOOK" Then
                BackwardsChinook(Stk, 3) = FramDataSet.Tables("BackwardsFRAM").Rows(RecNum)(2)
                BackwardsChinook(Stk, 4) = FramDataSet.Tables("BackwardsFRAM").Rows(RecNum)(3)
                BackwardsChinook(Stk, 5) = FramDataSet.Tables("BackwardsFRAM").Rows(RecNum)(4)
            End If
            BackwardsFlag(Stk) = FramDataSet.Tables("BackwardsFRAM").Rows(RecNum)(5)
            If FramDataSet.Tables("BackwardsFRAM").Columns.IndexOf("Comment") <> -1 Then
                BackwardsComment(Stk) = FramDataSet.Tables("BackwardsFRAM").Rows(RecNum)(6)
            End If
        Next
        BFDA = Nothing

        '========================================================================
        '- Get Calculation Arrays if RunTime is <> Default Value
        If RunIDRunTimeDateSelect.Date = #1/1/2001# Then GoTo SkipCalcArrays

        '- Read Mortality Data
        CmdStr = "SELECT * FROM Mortality WHERE RunID = " & RunIDSelect.ToString
        Dim Mcm As New SQLite.SQLiteCommand(CmdStr, FramDB)
        Dim MortalityDA As New System.Data.SQLite.SQLiteDataAdapter
        MortalityDA.SelectCommand = Mcm
        Dim Mcb As New SQLite.SQLiteCommandBuilder
        Mcb = New SQLite.SQLiteCommandBuilder(MortalityDA)
        If FramDataSet.Tables.Contains("Mortality") Then
            FramDataSet.Tables("Mortality").Clear()
        End If
        MortalityDA.Fill(FramDataSet, "Mortality")
        Dim NumM As Integer
        NumM = FramDataSet.Tables("Mortality").Rows.Count
        '- Loop through Table Records for Actual Values
        For RecNum = 0 To NumM - 1
            RunIDNum = FramDataSet.Tables("Mortality").Rows(RecNum)(1)
            Stk = FramDataSet.Tables("Mortality").Rows(RecNum)(2)
            Age = FramDataSet.Tables("Mortality").Rows(RecNum)(3)
            Fish = FramDataSet.Tables("Mortality").Rows(RecNum)(4)
            TStep = FramDataSet.Tables("Mortality").Rows(RecNum)(5)
            If Fish > NumFish Or TStep > NumSteps Then
                MsgBox("Error in Mortality Table Read", MsgBoxStyle.OkOnly)
            End If
            LandedCatch(Stk, Age, Fish, TStep) = FramDataSet.Tables("Mortality").Rows(RecNum)(6)
            NonRetention(Stk, Age, Fish, TStep) = FramDataSet.Tables("Mortality").Rows(RecNum)(7)
            Shakers(Stk, Age, Fish, TStep) = FramDataSet.Tables("Mortality").Rows(RecNum)(8)
            DropOff(Stk, Age, Fish, TStep) = FramDataSet.Tables("Mortality").Rows(RecNum)(9)
            Encounters(Stk, Age, Fish, TStep) = FramDataSet.Tables("Mortality").Rows(RecNum)(10)

            If IsDBNull(FramDataSet.Tables("Mortality").Rows(RecNum)(11)) Then
                MSFLandedCatch(Stk, Age, Fish, TStep) = 0
            Else
                MSFLandedCatch(Stk, Age, Fish, TStep) = FramDataSet.Tables("Mortality").Rows(RecNum)(11)
            End If
            If IsDBNull(FramDataSet.Tables("Mortality").Rows(RecNum)(12)) Then
                MSFNonRetention(Stk, Age, Fish, TStep) = 0
            Else
                MSFNonRetention(Stk, Age, Fish, TStep) = FramDataSet.Tables("Mortality").Rows(RecNum)(12)
            End If
            If IsDBNull(FramDataSet.Tables("Mortality").Rows(RecNum)(13)) Then
                MSFShakers(Stk, Age, Fish, TStep) = 0
            Else
                MSFShakers(Stk, Age, Fish, TStep) = FramDataSet.Tables("Mortality").Rows(RecNum)(13)
            End If
            If IsDBNull(FramDataSet.Tables("Mortality").Rows(RecNum)(14)) Then
                MSFDropOff(Stk, Age, Fish, TStep) = 0
            Else
                MSFDropOff(Stk, Age, Fish, TStep) = FramDataSet.Tables("Mortality").Rows(RecNum)(14)
            End If
            If IsDBNull(FramDataSet.Tables("Mortality").Rows(RecNum)(15)) Then
                MSFEncounters(Stk, Age, Fish, TStep) = 0
            Else
                MSFEncounters(Stk, Age, Fish, TStep) = FramDataSet.Tables("Mortality").Rows(RecNum)(15)
            End If
            'MSFLandedCatch(Stk, Age, Fish, TStep) = FramDataSet.Tables("Mortality").Rows(RecNum)(11)
            'MSFNonRetention(Stk, Age, Fish, TStep) = FramDataSet.Tables("Mortality").Rows(RecNum)(12)
            'MSFShakers(Stk, Age, Fish, TStep) = FramDataSet.Tables("Mortality").Rows(RecNum)(13)
            'MSFDropOff(Stk, Age, Fish, TStep) = FramDataSet.Tables("Mortality").Rows(RecNum)(14)
            'MSFEncounters(Stk, Age, Fish, TStep) = FramDataSet.Tables("Mortality").Rows(RecNum)(15)
        Next
        MortalityDA = Nothing

        '- Read Cohort Data
        CmdStr = "SELECT * FROM Cohort WHERE RunID = " & RunIDSelect.ToString
        Dim CScm As New SQLite.SQLiteCommand(CmdStr, FramDB)
        Dim CohortDA As New System.Data.SQLite.SQLiteDataAdapter
        CohortDA.SelectCommand = CScm
        Dim CScb As New SQLite.SQLiteCommandBuilder
        CScb = New SQLite.SQLiteCommandBuilder(CohortDA)
        If FramDataSet.Tables.Contains("Cohort") Then
            FramDataSet.Tables("Cohort").Clear()
        End If
        CohortDA.Fill(FramDataSet, "Cohort")
        Dim NumCS As Integer
        NumCS = FramDataSet.Tables("Cohort").Rows.Count
        '- Loop through Table Records for Actual Values
        For RecNum = 0 To NumCS - 1
            RunIDNum = FramDataSet.Tables("Cohort").Rows(RecNum)(1)
            Stk = FramDataSet.Tables("Cohort").Rows(RecNum)(2)
            Age = FramDataSet.Tables("Cohort").Rows(RecNum)(3)
            TStep = FramDataSet.Tables("Cohort").Rows(RecNum)(4)
            If Stk > NumStk Or TStep > NumSteps Then
                MsgBox("Error in Cohort Table Read", MsgBoxStyle.OkOnly)
            End If
            Cohort(Stk, Age, 0, TStep) = FramDataSet.Tables("Cohort").Rows(RecNum)(5)
            Cohort(Stk, Age, 1, TStep) = FramDataSet.Tables("Cohort").Rows(RecNum)(6)
            Cohort(Stk, Age, 4, TStep) = FramDataSet.Tables("Cohort").Rows(RecNum)(7)
            Cohort(Stk, Age, 3, TStep) = FramDataSet.Tables("Cohort").Rows(RecNum)(8)
            Cohort(Stk, Age, 2, TStep) = FramDataSet.Tables("Cohort").Rows(RecNum)(9)
        Next
        CohortDA = Nothing

        '- Read Escapement Data
        CmdStr = "SELECT * FROM Escapement WHERE RunID = " & RunIDSelect.ToString
        Dim EScm As New SQLite.SQLiteCommand(CmdStr, FramDB)
        Dim EscapementDA As New System.Data.SQLite.SQLiteDataAdapter
        EscapementDA.SelectCommand = EScm
        Dim EScb As New SQLite.SQLiteCommandBuilder
        EScb = New SQLite.SQLiteCommandBuilder(EscapementDA)
        If FramDataSet.Tables.Contains("Escapement") Then
            FramDataSet.Tables("Escapement").Clear()
        End If
        EscapementDA.Fill(FramDataSet, "Escapement")
        Dim NumES As Integer
        NumES = FramDataSet.Tables("Escapement").Rows.Count
        '- Loop through Table Records for Actual Values
        For RecNum = 0 To NumES - 1
            RunIDNum = FramDataSet.Tables("Escapement").Rows(RecNum)(1)
            Stk = FramDataSet.Tables("Escapement").Rows(RecNum)(2)
            Age = FramDataSet.Tables("Escapement").Rows(RecNum)(3)
            TStep = FramDataSet.Tables("Escapement").Rows(RecNum)(4)
            If Stk > NumStk Or TStep > NumSteps Then
                MsgBox("Error in Escapement Table Read", MsgBoxStyle.OkOnly)
            End If
            Escape(Stk, Age, TStep) = FramDataSet.Tables("Escapement").Rows(RecNum)(5)
        Next
        EscapementDA = Nothing

        '- Read Total Fishery Mortality Data
        CmdStr = "SELECT * FROM FisheryMortality WHERE RunID = " & RunIDSelect.ToString
        Dim TFMcm As New SQLite.SQLiteCommand(CmdStr, FramDB)
        Dim TFMDA As New System.Data.SQLite.SQLiteDataAdapter
        TFMDA.SelectCommand = TFMcm
        Dim TFMcb As New SQLite.SQLiteCommandBuilder
        TFMcb = New SQLite.SQLiteCommandBuilder(EscapementDA)
        If FramDataSet.Tables.Contains("FisheryMortality") Then
            FramDataSet.Tables("FisheryMortality").Clear()
        End If
        TFMDA.Fill(FramDataSet, "FisheryMortality")
        Dim TfmES As Integer
        TfmES = FramDataSet.Tables("FisheryMortality").Rows.Count
        '- Loop through Table Records for Actual Values
        For RecNum = 0 To TfmES - 1
            RunIDNum = FramDataSet.Tables("FisheryMortality").Rows(RecNum)(0)
            Fish = FramDataSet.Tables("FisheryMortality").Rows(RecNum)(1)
            TStep = FramDataSet.Tables("FisheryMortality").Rows(RecNum)(2)
            If Fish > NumFish Or TStep > NumSteps Then
                MsgBox("Error in FisheryMortality Table Read", MsgBoxStyle.OkOnly)
            End If
            TotalLandedCatch(Fish, TStep) = FramDataSet.Tables("FisheryMortality").Rows(RecNum)(3)
            TotalLandedCatch(NumFish + Fish, TStep) = FramDataSet.Tables("FisheryMortality").Rows(RecNum)(4)
            TotalNonRetention(Fish, TStep) = FramDataSet.Tables("FisheryMortality").Rows(RecNum)(5)
            TotalShakers(Fish, TStep) = FramDataSet.Tables("FisheryMortality").Rows(RecNum)(6)
            TotalDropOff(Fish, TStep) = FramDataSet.Tables("FisheryMortality").Rows(RecNum)(7)
            TotalEncounters(Fish, TStep) = FramDataSet.Tables("FisheryMortality").Rows(RecNum)(8)
        Next
        TFMDA = Nothing


        '- Read PSC Max ER Data for COHO
        'On Error Resume Next

        CmdStr = "SELECT * FROM PSCMaxER WHERE RunID = " & RunIDSelect.ToString

        Dim MEcm As New SQLite.SQLiteCommand(CmdStr, FramDB)
        Dim MEDA As New System.Data.SQLite.SQLiteDataAdapter
        MEDA.SelectCommand = MEcm
        Dim MEcb As New SQLite.SQLiteCommandBuilder
        MEcb = New SQLite.SQLiteCommandBuilder(MEDA)
        If FramDataSet.Tables.Contains("PSCMaxER") Then
            FramDataSet.Tables("PSCMaxER").Clear()
        End If
        ' Try
        MEDA.Fill(FramDataSet, "PSCMaxER")
        'Catch Ex As Exception
        'GoTo SkipCalcArrays
        'End Try

        Dim NumME As Integer
        NumME = FramDataSet.Tables("PSCMaxER").Rows.Count
        '- Loop through Table Records for Actual Values
        For RecNum = 0 To NumME - 1
            RunIDNum = FramDataSet.Tables("PSCMaxER").Rows(RecNum)(0)
            Stk = FramDataSet.Tables("PSCMaxER").Rows(RecNum)(1)
            If Stk > 17 Then
                MsgBox("Error in PSCMaxER Table Read", MsgBoxStyle.OkOnly)
            End If
            PSCMaxER(Stk) = FramDataSet.Tables("PSCMaxER").Rows(RecNum)(2)
        Next
        '- If No PSC Max ER Records Exist Create Placeholders (Used Only for Reports)
        If NumME = 0 Then
            For Stk As Integer = 1 To 17
                PSCMaxER(Stk) = 0.5
            Next
        End If
        MEDA = Nothing


        '- End of Calc Array Read =======================================================

SkipCalcArrays:
        rssw.Flush()
        rssw.Close()
        FramDB.Close()

        Me.Cursor = Cursors.Default

    End Sub

    Public Sub LoadRunArrays()

        ' this loads all the runs in the database 
        ' into arrays

        Dim FramDB As New SQLite.SQLiteConnection("Data Source=" & FVSdatabasename & ";Version=3;Compress=True;")
        FramDB.Open()
        Dim drd1 As SQLite.SQLiteDataReader
        Dim cmd1 As New SQLite.SQLiteCommand()
        cmd1.Connection = FramDB
        cmd1.CommandText = "SELECT * FROM RunID ORDER BY RunID"
        drd1 = cmd1.ExecuteReader
        Dim str1 As String
        Dim int1 As Integer
        int1 = 0
        If drd1.HasRows = False Then
            '- No RunID Recordsets .. Must Read Old CMD File
            MsgBox("No runs in the database...")
        End If
        Do While drd1.Read
            '- Fill CheckedListBox Items
            '- Set RunID Array Values
            RunID(int1) = drd1.GetInt32(1)
            RunBasePeriodID(int1) = drd1.GetInt32(5)
            RunIDName(int1) = drd1.GetString(3)
            int1 = int1 + 1
        Loop
        FramDB.Close()
        ReDim Preserve RunID(int1 - 1)
        ReDim Preserve RunBasePeriodID(int1 - 1)
        ReDim Preserve RunIDName(int1 - 1)
    End Sub
End Class