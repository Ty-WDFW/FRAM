<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FVS_SQLite
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.ListBox1 = New System.Windows.Forms.ListBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.RSTitle = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(559, 621)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(189, 58)
        Me.Button1.TabIndex = 0
        Me.Button1.Text = "Test"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'ListBox1
        '
        Me.ListBox1.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.ListBox1.Font = New System.Drawing.Font("Courier New", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ListBox1.FormattingEnabled = True
        Me.ListBox1.ItemHeight = 17
        Me.ListBox1.Location = New System.Drawing.Point(119, 170)
        Me.ListBox1.Name = "ListBox1"
        Me.ListBox1.Size = New System.Drawing.Size(1069, 429)
        Me.ListBox1.TabIndex = 9
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Courier New", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(116, 151)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(431, 16)
        Me.Label2.TabIndex = 8
        Me.Label2.Text = "Index Species  Title                      Description"
        '
        'RSTitle
        '
        Me.RSTitle.AutoSize = True
        Me.RSTitle.Font = New System.Drawing.Font("Microsoft Sans Serif", 16.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RSTitle.Location = New System.Drawing.Point(498, 90)
        Me.RSTitle.Name = "RSTitle"
        Me.RSTitle.Size = New System.Drawing.Size(305, 26)
        Me.RSTitle.TabIndex = 7
        Me.RSTitle.Text = "FRAM Model Run Selection"
        '
        'FVS_SQLite
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1315, 741)
        Me.Controls.Add(Me.ListBox1)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.RSTitle)
        Me.Controls.Add(Me.Button1)
        Me.Name = "FVS_SQLite"
        Me.Text = "Form1"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Button1 As Button
    Friend WithEvents ListBox1 As ListBox
    Friend WithEvents Label2 As Label
    Friend WithEvents RSTitle As Label
End Class
