<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmPicSelect
#Region "Windows Form Designer generated code "
	<System.Diagnostics.DebuggerNonUserCode()> Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
	End Sub
	'Form overrides dispose to clean up the component list.
	<System.Diagnostics.DebuggerNonUserCode()> Protected Overloads Overrides Sub Dispose(ByVal Disposing As Boolean)
		If Disposing Then
			If Not components Is Nothing Then
				components.Dispose()
			End If
		End If
		MyBase.Dispose(Disposing)
	End Sub
	'Required by the Windows Form Designer
	Private components As System.ComponentModel.IContainer
	Public ToolTip1 As System.Windows.Forms.ToolTip
	Public WithEvents chkManual As System.Windows.Forms.CheckBox
	Public WithEvents chkShoComt As System.Windows.Forms.CheckBox
	Public WithEvents chkShoDate As System.Windows.Forms.CheckBox
	Public WithEvents chkShoPath As System.Windows.Forms.CheckBox
  Public WithEvents chkShoFName As System.Windows.Forms.CheckBox
	Public WithEvents cmdExit As System.Windows.Forms.Button
	Public WithEvents cmdSelOK As System.Windows.Forms.Button
  Public WithEvents xLblUDMajDesc As System.Windows.Forms.Label
  'NOTE: The following procedure is required by the Windows Form Designer
  'It can be modified using the Windows Form Designer.
  'Do not modify it using the code editor.
  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmPicSelect))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.xLblUDMajDesc = New System.Windows.Forms.Label()
        Me.udMajor = New System.Windows.Forms.NumericUpDown()
        Me.cmdShoSettgs = New System.Windows.Forms.Button()
        Me.chkFadeWanted = New System.Windows.Forms.CheckBox()
        Me.udMinor = New System.Windows.Forms.NumericUpDown()
        Me.xLblUDMinDesc = New System.Windows.Forms.Label()
        Me.chkSubs = New System.Windows.Forms.CheckBox()
        Me.radioByDate = New System.Windows.Forms.RadioButton()
        Me.radioRandom = New System.Windows.Forms.RadioButton()
        Me.radioByName = New System.Windows.Forms.RadioButton()
        Me.chkShoDate = New System.Windows.Forms.CheckBox()
        Me.chkShoPath = New System.Windows.Forms.CheckBox()
        Me.chkManual = New System.Windows.Forms.CheckBox()
        Me.chkShoComt = New System.Windows.Forms.CheckBox()
        Me.chkShoFName = New System.Windows.Forms.CheckBox()
        Me.cmdExit = New System.Windows.Forms.Button()
        Me.cmdSelOK = New System.Windows.Forms.Button()
        Me.chkTrimFName = New System.Windows.Forms.CheckBox()
        Me.lstDirs2Vu = New System.Windows.Forms.ListBox()
        Me.cmdAddDirs = New System.Windows.Forms.Button()
        Me.grpboxSort = New System.Windows.Forms.GroupBox()
        CType(Me.udMajor, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.udMinor, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpboxSort.SuspendLayout()
        Me.SuspendLayout()
        '
        'xLblUDMajDesc
        '
        Me.xLblUDMajDesc.AutoSize = True
        Me.xLblUDMajDesc.BackColor = System.Drawing.Color.LightGray
        Me.xLblUDMajDesc.Cursor = System.Windows.Forms.Cursors.Default
        Me.xLblUDMajDesc.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.xLblUDMajDesc.ForeColor = System.Drawing.SystemColors.ControlText
        Me.xLblUDMajDesc.Location = New System.Drawing.Point(72, 360)
        Me.xLblUDMajDesc.Name = "xLblUDMajDesc"
        Me.xLblUDMajDesc.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.xLblUDMajDesc.Size = New System.Drawing.Size(176, 16)
        Me.xLblUDMajDesc.TabIndex = 3
        Me.xLblUDMajDesc.Text = "Seconds per image (1 - 30)"
        Me.ToolTip1.SetToolTip(Me.xLblUDMajDesc, "Length of time each image should appear")
        '
        'udMajor
        '
        Me.udMajor.Location = New System.Drawing.Point(24, 360)
        Me.udMajor.Maximum = New Decimal(New Integer() {30, 0, 0, 0})
        Me.udMajor.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.udMajor.Name = "udMajor"
        Me.udMajor.Size = New System.Drawing.Size(40, 22)
        Me.udMajor.TabIndex = 13
        Me.ToolTip1.SetToolTip(Me.udMajor, "Length of time each image should appear")
        Me.udMajor.Value = New Decimal(New Integer() {5, 0, 0, 0})
        '
        'cmdShoSettgs
        '
        Me.cmdShoSettgs.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.cmdShoSettgs.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdShoSettgs.Location = New System.Drawing.Point(168, 24)
        Me.cmdShoSettgs.Name = "cmdShoSettgs"
        Me.cmdShoSettgs.Size = New System.Drawing.Size(94, 62)
        Me.cmdShoSettgs.TabIndex = 14
        Me.cmdShoSettgs.Text = "Video settings"
        Me.ToolTip1.SetToolTip(Me.cmdShoSettgs, "Display current video settings")
        Me.cmdShoSettgs.UseVisualStyleBackColor = True
        '
        'chkFadeWanted
        '
        Me.chkFadeWanted.AutoSize = True
        Me.chkFadeWanted.BackColor = System.Drawing.Color.LightGray
        Me.chkFadeWanted.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkFadeWanted.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkFadeWanted.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkFadeWanted.Location = New System.Drawing.Point(24, 408)
        Me.chkFadeWanted.Name = "chkFadeWanted"
        Me.chkFadeWanted.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkFadeWanted.Size = New System.Drawing.Size(168, 20)
        Me.chkFadeWanted.TabIndex = 16
        Me.chkFadeWanted.Text = "Fade between images"
        Me.ToolTip1.SetToolTip(Me.chkFadeWanted, "Select gradual transition")
        Me.chkFadeWanted.UseVisualStyleBackColor = False
        '
        'udMinor
        '
        Me.udMinor.Enabled = False
        Me.udMinor.Location = New System.Drawing.Point(24, 448)
        Me.udMinor.Maximum = New Decimal(New Integer() {8, 0, 0, 0})
        Me.udMinor.Minimum = New Decimal(New Integer() {2, 0, 0, 0})
        Me.udMinor.Name = "udMinor"
        Me.udMinor.Size = New System.Drawing.Size(40, 22)
        Me.udMinor.TabIndex = 18
        Me.ToolTip1.SetToolTip(Me.udMinor, "Length of time for optional fade")
        Me.udMinor.Value = New Decimal(New Integer() {3, 0, 0, 0})
        '
        'xLblUDMinDesc
        '
        Me.xLblUDMinDesc.AutoSize = True
        Me.xLblUDMinDesc.BackColor = System.Drawing.Color.LightGray
        Me.xLblUDMinDesc.Cursor = System.Windows.Forms.Cursors.Default
        Me.xLblUDMinDesc.Enabled = False
        Me.xLblUDMinDesc.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.xLblUDMinDesc.ForeColor = System.Drawing.SystemColors.ControlText
        Me.xLblUDMinDesc.Location = New System.Drawing.Point(72, 448)
        Me.xLblUDMinDesc.Name = "xLblUDMinDesc"
        Me.xLblUDMinDesc.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.xLblUDMinDesc.Size = New System.Drawing.Size(176, 16)
        Me.xLblUDMinDesc.TabIndex = 17
        Me.xLblUDMinDesc.Text = "Seconds per transition (2 - 8)"
        Me.ToolTip1.SetToolTip(Me.xLblUDMinDesc, "Length of time for optional fade")
        '
        'chkSubs
        '
        Me.chkSubs.AutoSize = True
        Me.chkSubs.BackColor = System.Drawing.Color.LightGray
        Me.chkSubs.Checked = True
        Me.chkSubs.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkSubs.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkSubs.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkSubs.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkSubs.Location = New System.Drawing.Point(280, 512)
        Me.chkSubs.Name = "chkSubs"
        Me.chkSubs.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkSubs.Size = New System.Drawing.Size(143, 20)
        Me.chkSubs.TabIndex = 21
        Me.chkSubs.Text = "Include subfolders"
        Me.ToolTip1.SetToolTip(Me.chkSubs, "Show images in subfolders")
        Me.chkSubs.UseVisualStyleBackColor = False
        '
        'radioByDate
        '
        Me.radioByDate.AutoSize = True
        Me.radioByDate.Location = New System.Drawing.Point(31, 58)
        Me.radioByDate.Name = "radioByDate"
        Me.radioByDate.Size = New System.Drawing.Size(71, 20)
        Me.radioByDate.TabIndex = 0
        Me.radioByDate.Text = "By date"
        Me.ToolTip1.SetToolTip(Me.radioByDate, "By date taken, or file modified")
        Me.radioByDate.UseVisualStyleBackColor = True
        '
        'radioRandom
        '
        Me.radioRandom.AutoSize = True
        Me.radioRandom.Location = New System.Drawing.Point(31, 88)
        Me.radioRandom.Name = "radioRandom"
        Me.radioRandom.Size = New System.Drawing.Size(74, 20)
        Me.radioRandom.TabIndex = 1
        Me.radioRandom.Text = "Random"
        Me.ToolTip1.SetToolTip(Me.radioRandom, "Display in random order")
        Me.radioRandom.UseVisualStyleBackColor = True
        '
        'radioByName
        '
        Me.radioByName.AutoSize = True
        Me.radioByName.Checked = True
        Me.radioByName.Location = New System.Drawing.Point(31, 27)
        Me.radioByName.Name = "radioByName"
        Me.radioByName.Size = New System.Drawing.Size(78, 20)
        Me.radioByName.TabIndex = 2
        Me.radioByName.TabStop = True
        Me.radioByName.Text = "By name"
        Me.ToolTip1.SetToolTip(Me.radioByName, "Default: by folder and file name")
        Me.radioByName.UseVisualStyleBackColor = True
        '
        'chkShoDate
        '
        Me.chkShoDate.AutoSize = True
        Me.chkShoDate.BackColor = System.Drawing.Color.LightGray
        Me.chkShoDate.Checked = True
        Me.chkShoDate.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkShoDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkShoDate.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkShoDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkShoDate.Location = New System.Drawing.Point(24, 216)
        Me.chkShoDate.Name = "chkShoDate"
        Me.chkShoDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkShoDate.Size = New System.Drawing.Size(153, 20)
        Me.chkShoDate.TabIndex = 10
        Me.chkShoDate.Text = "Display picture date"
        Me.ToolTip1.SetToolTip(Me.chkShoDate, "Date taken if available, or date modified")
        Me.chkShoDate.UseVisualStyleBackColor = False
        '
        'chkShoPath
        '
        Me.chkShoPath.AutoSize = True
        Me.chkShoPath.BackColor = System.Drawing.Color.LightGray
        Me.chkShoPath.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkShoPath.Enabled = False
        Me.chkShoPath.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkShoPath.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkShoPath.Location = New System.Drawing.Point(40, 152)
        Me.chkShoPath.Name = "chkShoPath"
        Me.chkShoPath.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkShoPath.Size = New System.Drawing.Size(149, 20)
        Me.chkShoPath.TabIndex = 9
        Me.chkShoPath.Text = "Display path && type"
        Me.ToolTip1.SetToolTip(Me.chkShoPath, "Show full file path and type")
        Me.chkShoPath.UseVisualStyleBackColor = False
        '
        'chkManual
        '
        Me.chkManual.AutoSize = True
        Me.chkManual.BackColor = System.Drawing.Color.LightGray
        Me.chkManual.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkManual.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkManual.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkManual.Location = New System.Drawing.Point(24, 301)
        Me.chkManual.Name = "chkManual"
        Me.chkManual.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkManual.Size = New System.Drawing.Size(193, 20)
        Me.chkManual.TabIndex = 12
        Me.chkManual.Text = "Run under manual control"
        Me.chkManual.UseVisualStyleBackColor = False
        '
        'chkShoComt
        '
        Me.chkShoComt.AutoSize = True
        Me.chkShoComt.BackColor = System.Drawing.Color.LightGray
        Me.chkShoComt.Checked = True
        Me.chkShoComt.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkShoComt.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkShoComt.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkShoComt.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkShoComt.Location = New System.Drawing.Point(24, 248)
        Me.chkShoComt.Name = "chkShoComt"
        Me.chkShoComt.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkShoComt.Size = New System.Drawing.Size(175, 20)
        Me.chkShoComt.TabIndex = 11
        Me.chkShoComt.Text = "Display comment if any"
        Me.chkShoComt.UseVisualStyleBackColor = False
        '
        'chkShoFName
        '
        Me.chkShoFName.AutoSize = True
        Me.chkShoFName.BackColor = System.Drawing.Color.LightGray
        Me.chkShoFName.Checked = True
        Me.chkShoFName.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkShoFName.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkShoFName.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkShoFName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkShoFName.Location = New System.Drawing.Point(24, 120)
        Me.chkShoFName.Name = "chkShoFName"
        Me.chkShoFName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkShoFName.Size = New System.Drawing.Size(139, 20)
        Me.chkShoFName.TabIndex = 6
        Me.chkShoFName.Text = "Display filenames"
        Me.chkShoFName.UseVisualStyleBackColor = False
        '
        'cmdExit
        '
        Me.cmdExit.AutoSize = True
        Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdExit.Font = New System.Drawing.Font("Arial", 13.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExit.Location = New System.Drawing.Point(328, 24)
        Me.cmdExit.Name = "cmdExit"
        Me.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdExit.Size = New System.Drawing.Size(77, 52)
        Me.cmdExit.TabIndex = 5
        Me.cmdExit.Text = "EXIT"
        Me.cmdExit.UseVisualStyleBackColor = False
        '
        'cmdSelOK
        '
        Me.cmdSelOK.AutoSize = True
        Me.cmdSelOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSelOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSelOK.Font = New System.Drawing.Font("Arial", 13.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSelOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSelOK.Location = New System.Drawing.Point(24, 24)
        Me.cmdSelOK.Name = "cmdSelOK"
        Me.cmdSelOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSelOK.Size = New System.Drawing.Size(73, 39)
        Me.cmdSelOK.TabIndex = 4
        Me.cmdSelOK.Text = "O K"
        Me.cmdSelOK.UseVisualStyleBackColor = False
        '
        'chkTrimFName
        '
        Me.chkTrimFName.AutoSize = True
        Me.chkTrimFName.BackColor = System.Drawing.Color.LightGray
        Me.chkTrimFName.Checked = True
        Me.chkTrimFName.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkTrimFName.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkTrimFName.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkTrimFName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkTrimFName.Location = New System.Drawing.Point(40, 184)
        Me.chkTrimFName.Name = "chkTrimFName"
        Me.chkTrimFName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkTrimFName.Size = New System.Drawing.Size(115, 20)
        Me.chkTrimFName.TabIndex = 15
        Me.chkTrimFName.Text = "&Trim filename"
        Me.chkTrimFName.UseVisualStyleBackColor = False
        '
        'lstDirs2Vu
        '
        Me.lstDirs2Vu.FormattingEnabled = True
        Me.lstDirs2Vu.HorizontalScrollbar = True
        Me.lstDirs2Vu.ItemHeight = 16
        Me.lstDirs2Vu.Location = New System.Drawing.Point(24, 568)
        Me.lstDirs2Vu.Name = "lstDirs2Vu"
        Me.lstDirs2Vu.Size = New System.Drawing.Size(440, 100)
        Me.lstDirs2Vu.TabIndex = 19
        '
        'cmdAddDirs
        '
        Me.cmdAddDirs.AutoSize = True
        Me.cmdAddDirs.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAddDirs.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAddDirs.Font = New System.Drawing.Font("Arial", 10.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAddDirs.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAddDirs.Location = New System.Drawing.Point(24, 504)
        Me.cmdAddDirs.Name = "cmdAddDirs"
        Me.cmdAddDirs.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAddDirs.Size = New System.Drawing.Size(216, 34)
        Me.cmdAddDirs.TabIndex = 20
        Me.cmdAddDirs.Text = "Add folder(s) to view"
        Me.cmdAddDirs.UseVisualStyleBackColor = False
        '
        'grpboxSort
        '
        Me.grpboxSort.AutoSize = True
        Me.grpboxSort.Controls.Add(Me.radioByName)
        Me.grpboxSort.Controls.Add(Me.radioRandom)
        Me.grpboxSort.Controls.Add(Me.radioByDate)
        Me.grpboxSort.Location = New System.Drawing.Point(302, 280)
        Me.grpboxSort.Name = "grpboxSort"
        Me.grpboxSort.Size = New System.Drawing.Size(167, 136)
        Me.grpboxSort.TabIndex = 22
        Me.grpboxSort.TabStop = False
        Me.grpboxSort.Text = "Sorting:"
        '
        'frmPicSelect
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.LightGray
        Me.ClientSize = New System.Drawing.Size(499, 727)
        Me.ControlBox = False
        Me.Controls.Add(Me.grpboxSort)
        Me.Controls.Add(Me.chkSubs)
        Me.Controls.Add(Me.cmdAddDirs)
        Me.Controls.Add(Me.lstDirs2Vu)
        Me.Controls.Add(Me.udMinor)
        Me.Controls.Add(Me.xLblUDMinDesc)
        Me.Controls.Add(Me.chkFadeWanted)
        Me.Controls.Add(Me.chkTrimFName)
        Me.Controls.Add(Me.cmdShoSettgs)
        Me.Controls.Add(Me.udMajor)
        Me.Controls.Add(Me.chkManual)
        Me.Controls.Add(Me.chkShoComt)
        Me.Controls.Add(Me.chkShoDate)
        Me.Controls.Add(Me.chkShoPath)
        Me.Controls.Add(Me.chkShoFName)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.cmdSelOK)
        Me.Controls.Add(Me.xLblUDMajDesc)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(162, 20)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmPicSelect"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Select image-display options"
        CType(Me.udMajor, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.udMinor, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpboxSort.ResumeLayout(False)
        Me.grpboxSort.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
 Friend WithEvents udMajor As System.Windows.Forms.NumericUpDown
 Friend WithEvents cmdShoSettgs As System.Windows.Forms.Button
 Public WithEvents chkTrimFName As System.Windows.Forms.CheckBox
 Public WithEvents chkFadeWanted As System.Windows.Forms.CheckBox
 Friend WithEvents udMinor As System.Windows.Forms.NumericUpDown
 Public WithEvents xLblUDMinDesc As System.Windows.Forms.Label
 Friend WithEvents lstDirs2Vu As System.Windows.Forms.ListBox
 Public WithEvents cmdAddDirs As System.Windows.Forms.Button
 Public WithEvents chkSubs As System.Windows.Forms.CheckBox
 Friend WithEvents grpboxSort As System.Windows.Forms.GroupBox
 Friend WithEvents radioRandom As System.Windows.Forms.RadioButton
 Friend WithEvents radioByDate As System.Windows.Forms.RadioButton
 Friend WithEvents radioByName As System.Windows.Forms.RadioButton
#End Region 
End Class