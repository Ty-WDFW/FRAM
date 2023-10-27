<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FVS_MainMenu
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.FramVersionLabel = New System.Windows.Forms.Label()
        Me.OpenDB = New System.Windows.Forms.Button()
        Me.ModelRun = New System.Windows.Forms.Button()
        Me.FVS_Exit = New System.Windows.Forms.Button()
        Me.OpenFVSdatabase = New System.Windows.Forms.OpenFileDialog()
        Me.DatabaseTextLabel = New System.Windows.Forms.Label()
        Me.RecordSetTextLabel = New System.Windows.Forms.Label()
        Me.RecordSetName = New System.Windows.Forms.Label()
        Me.DbName = New System.Windows.Forms.Label()
        Me.DatabaseNameLabel = New System.Windows.Forms.Label()
        Me.RecordSetNameLabel = New System.Windows.Forms.Label()
        Me.HelpProvider1 = New System.Windows.Forms.HelpProvider()
        Me.SelectRecordset = New System.Windows.Forms.Button()
        Me.RunBatch = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 16.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(199, 33)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(419, 26)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Fishery Regulation Assessment Model"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 16.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(344, 119)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(128, 26)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Main Menu"
        '
        'FramVersionLabel
        '
        Me.FramVersionLabel.AutoSize = True
        Me.FramVersionLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 16.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FramVersionLabel.Location = New System.Drawing.Point(335, 76)
        Me.FramVersionLabel.Name = "FramVersionLabel"
        Me.FramVersionLabel.Size = New System.Drawing.Size(146, 26)
        Me.FramVersionLabel.TabIndex = 3
        Me.FramVersionLabel.Text = "Version 6.00"
        '
        'OpenDB
        '
        Me.OpenDB.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.OpenDB.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OpenDB.Location = New System.Drawing.Point(279, 174)
        Me.OpenDB.Name = "OpenDB"
        Me.OpenDB.Size = New System.Drawing.Size(258, 50)
        Me.OpenDB.TabIndex = 4
        Me.OpenDB.Text = "Open Database"
        Me.OpenDB.UseVisualStyleBackColor = False
        '
        'ModelRun
        '
        Me.ModelRun.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.ModelRun.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ModelRun.Location = New System.Drawing.Point(279, 308)
        Me.ModelRun.Name = "ModelRun"
        Me.ModelRun.Size = New System.Drawing.Size(258, 50)
        Me.ModelRun.TabIndex = 6
        Me.ModelRun.Text = "Run Model "
        Me.ModelRun.UseVisualStyleBackColor = False
        '
        'FVS_Exit
        '
        Me.FVS_Exit.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.FVS_Exit.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FVS_Exit.Location = New System.Drawing.Point(278, 465)
        Me.FVS_Exit.Name = "FVS_Exit"
        Me.FVS_Exit.Size = New System.Drawing.Size(258, 50)
        Me.FVS_Exit.TabIndex = 9
        Me.FVS_Exit.Text = "EXIT"
        Me.FVS_Exit.UseVisualStyleBackColor = False
        '
        'OpenFVSdatabase
        '
        Me.OpenFVSdatabase.DefaultExt = "*.mdb"
        Me.OpenFVSdatabase.FileName = "OpenFileDialog1"
        Me.OpenFVSdatabase.Filter = "database files (*.mdb)|*.mdb"
        '
        'DatabaseTextLabel
        '
        Me.DatabaseTextLabel.AutoSize = True
        Me.DatabaseTextLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DatabaseTextLabel.Location = New System.Drawing.Point(54, 538)
        Me.DatabaseTextLabel.Name = "DatabaseTextLabel"
        Me.DatabaseTextLabel.Size = New System.Drawing.Size(61, 13)
        Me.DatabaseTextLabel.TabIndex = 10
        Me.DatabaseTextLabel.Text = "Database"
        '
        'RecordSetTextLabel
        '
        Me.RecordSetTextLabel.AutoSize = True
        Me.RecordSetTextLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RecordSetTextLabel.Location = New System.Drawing.Point(54, 572)
        Me.RecordSetTextLabel.Name = "RecordSetTextLabel"
        Me.RecordSetTextLabel.Size = New System.Drawing.Size(67, 13)
        Me.RecordSetTextLabel.TabIndex = 11
        Me.RecordSetTextLabel.Text = "RecordSet"
        '
        'RecordSetName
        '
        Me.RecordSetName.AutoSize = True
        Me.RecordSetName.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RecordSetName.Location = New System.Drawing.Point(244, 572)
        Me.RecordSetName.Name = "RecordSetName"
        Me.RecordSetName.Size = New System.Drawing.Size(0, 13)
        Me.RecordSetName.TabIndex = 12
        '
        'DbName
        '
        Me.DbName.AutoSize = True
        Me.DbName.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DbName.Location = New System.Drawing.Point(244, 538)
        Me.DbName.Name = "DbName"
        Me.DbName.Size = New System.Drawing.Size(0, 13)
        Me.DbName.TabIndex = 13
        '
        'DatabaseNameLabel
        '
        Me.DatabaseNameLabel.AutoSize = True
        Me.DatabaseNameLabel.BackColor = System.Drawing.Color.Yellow
        Me.DatabaseNameLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DatabaseNameLabel.Location = New System.Drawing.Point(137, 538)
        Me.DatabaseNameLabel.Name = "DatabaseNameLabel"
        Me.DatabaseNameLabel.Size = New System.Drawing.Size(119, 17)
        Me.DatabaseNameLabel.TabIndex = 14
        Me.DatabaseNameLabel.Text = "database name"
        '
        'RecordSetNameLabel
        '
        Me.RecordSetNameLabel.AutoSize = True
        Me.RecordSetNameLabel.BackColor = System.Drawing.Color.Yellow
        Me.RecordSetNameLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RecordSetNameLabel.Location = New System.Drawing.Point(137, 572)
        Me.RecordSetNameLabel.Name = "RecordSetNameLabel"
        Me.RecordSetNameLabel.Size = New System.Drawing.Size(121, 17)
        Me.RecordSetNameLabel.TabIndex = 15
        Me.RecordSetNameLabel.Text = "recordset name"
        '
        'SelectRecordset
        '
        Me.SelectRecordset.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.SelectRecordset.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SelectRecordset.Location = New System.Drawing.Point(278, 243)
        Me.SelectRecordset.Name = "SelectRecordset"
        Me.SelectRecordset.Size = New System.Drawing.Size(258, 50)
        Me.SelectRecordset.TabIndex = 17
        Me.SelectRecordset.Text = "Select Model Run"
        Me.SelectRecordset.UseVisualStyleBackColor = False
        '
        'RunBatch
        '
        Me.RunBatch.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.RunBatch.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RunBatch.Location = New System.Drawing.Point(279, 375)
        Me.RunBatch.Name = "RunBatch"
        Me.RunBatch.Size = New System.Drawing.Size(258, 50)
        Me.RunBatch.TabIndex = 18
        Me.RunBatch.Text = "Batch Run"
        Me.RunBatch.UseVisualStyleBackColor = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(274, 428)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(272, 25)
        Me.Label2.TabIndex = 19
        Me.Label2.Text = "Running run number X of Y"
        Me.Label2.Visible = False
        '
        'FVS_MainMenu
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.AutoScroll = True
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(833, 600)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.RunBatch)
        Me.Controls.Add(Me.SelectRecordset)
        Me.Controls.Add(Me.RecordSetNameLabel)
        Me.Controls.Add(Me.DatabaseNameLabel)
        Me.Controls.Add(Me.DbName)
        Me.Controls.Add(Me.RecordSetName)
        Me.Controls.Add(Me.RecordSetTextLabel)
        Me.Controls.Add(Me.DatabaseTextLabel)
        Me.Controls.Add(Me.FVS_Exit)
        Me.Controls.Add(Me.ModelRun)
        Me.Controls.Add(Me.OpenDB)
        Me.Controls.Add(Me.FramVersionLabel)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label1)
        Me.HelpProvider1.SetHelpString(Me, "Main Menu is focal point for this application")
        Me.Name = "FVS_MainMenu"
        Me.HelpProvider1.SetShowHelp(Me, True)
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "FVS_MainMenu"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
   Friend WithEvents Label3 As System.Windows.Forms.Label
   Friend WithEvents FramVersionLabel As System.Windows.Forms.Label
   Friend WithEvents OpenDB As System.Windows.Forms.Button
    Friend WithEvents ModelRun As System.Windows.Forms.Button
    Friend WithEvents FVS_Exit As System.Windows.Forms.Button
    Friend WithEvents OpenFVSdatabase As System.Windows.Forms.OpenFileDialog
    Friend WithEvents DatabaseTextLabel As System.Windows.Forms.Label
    Friend WithEvents RecordSetTextLabel As System.Windows.Forms.Label
    Friend WithEvents RecordSetName As System.Windows.Forms.Label
    Friend WithEvents DbName As System.Windows.Forms.Label
    Friend WithEvents DatabaseNameLabel As System.Windows.Forms.Label
    Friend WithEvents RecordSetNameLabel As System.Windows.Forms.Label
    Friend WithEvents HelpProvider1 As System.Windows.Forms.HelpProvider
    Friend WithEvents SelectRecordset As System.Windows.Forms.Button
    Friend WithEvents RunBatch As Button
    Friend WithEvents Label2 As Label
End Class
