<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmPicShow
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
	Public WithEvents btPlay As System.Windows.Forms.PictureBox
	Public WithEvents btPrev As System.Windows.Forms.PictureBox
	Public WithEvents btNext As System.Windows.Forms.PictureBox
	Public WithEvents btExit As System.Windows.Forms.PictureBox
  Public WithEvents timrPShow As System.Windows.Forms.Timer
  'NOTE: The following procedure is required by the Windows Form Designer
  'It can be modified using the Windows Form Designer.
  'Do not modify it using the code editor.
  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
Me.components = New System.ComponentModel.Container
Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmPicShow))
Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
Me.btPlay = New System.Windows.Forms.PictureBox
Me.btPrev = New System.Windows.Forms.PictureBox
Me.btNext = New System.Windows.Forms.PictureBox
Me.btExit = New System.Windows.Forms.PictureBox
Me.timrPShow = New System.Windows.Forms.Timer(Me.components)
CType(Me.btPlay, System.ComponentModel.ISupportInitialize).BeginInit()
CType(Me.btPrev, System.ComponentModel.ISupportInitialize).BeginInit()
CType(Me.btNext, System.ComponentModel.ISupportInitialize).BeginInit()
CType(Me.btExit, System.ComponentModel.ISupportInitialize).BeginInit()
Me.SuspendLayout()
'
'btPlay
'
Me.btPlay.BackColor = System.Drawing.SystemColors.Window
Me.btPlay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
Me.btPlay.Cursor = System.Windows.Forms.Cursors.Default
Me.btPlay.Enabled = False
Me.btPlay.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
Me.btPlay.ForeColor = System.Drawing.SystemColors.WindowText
Me.btPlay.Image = CType(resources.GetObject("btPlay.Image"), System.Drawing.Image)
Me.btPlay.Location = New System.Drawing.Point(632, 552)
Me.btPlay.Name = "btPlay"
Me.btPlay.RightToLeft = System.Windows.Forms.RightToLeft.No
Me.btPlay.Size = New System.Drawing.Size(34, 34)
Me.btPlay.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
Me.btPlay.TabIndex = 7
Me.btPlay.TabStop = False
Me.btPlay.Visible = False
'
'btPrev
'
Me.btPrev.BackColor = System.Drawing.SystemColors.Window
Me.btPrev.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
Me.btPrev.Cursor = System.Windows.Forms.Cursors.Default
Me.btPrev.Enabled = False
Me.btPrev.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
Me.btPrev.ForeColor = System.Drawing.SystemColors.WindowText
Me.btPrev.Image = CType(resources.GetObject("btPrev.Image"), System.Drawing.Image)
Me.btPrev.Location = New System.Drawing.Point(672, 552)
Me.btPrev.Name = "btPrev"
Me.btPrev.RightToLeft = System.Windows.Forms.RightToLeft.No
Me.btPrev.Size = New System.Drawing.Size(34, 34)
Me.btPrev.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
Me.btPrev.TabIndex = 6
Me.btPrev.TabStop = False
Me.btPrev.Visible = False
'
'btNext
'
Me.btNext.BackColor = System.Drawing.SystemColors.Window
Me.btNext.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
Me.btNext.Cursor = System.Windows.Forms.Cursors.Default
Me.btNext.Enabled = False
Me.btNext.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
Me.btNext.ForeColor = System.Drawing.SystemColors.WindowText
Me.btNext.Image = CType(resources.GetObject("btNext.Image"), System.Drawing.Image)
Me.btNext.Location = New System.Drawing.Point(712, 552)
Me.btNext.Name = "btNext"
Me.btNext.RightToLeft = System.Windows.Forms.RightToLeft.No
Me.btNext.Size = New System.Drawing.Size(34, 34)
Me.btNext.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
Me.btNext.TabIndex = 5
Me.btNext.TabStop = False
Me.btNext.Visible = False
'
'btExit
'
Me.btExit.BackColor = System.Drawing.SystemColors.Window
Me.btExit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
Me.btExit.Cursor = System.Windows.Forms.Cursors.Default
Me.btExit.Enabled = False
Me.btExit.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
Me.btExit.ForeColor = System.Drawing.SystemColors.WindowText
Me.btExit.Image = CType(resources.GetObject("btExit.Image"), System.Drawing.Image)
Me.btExit.Location = New System.Drawing.Point(752, 552)
Me.btExit.Name = "btExit"
Me.btExit.RightToLeft = System.Windows.Forms.RightToLeft.No
Me.btExit.Size = New System.Drawing.Size(34, 34)
Me.btExit.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
Me.btExit.TabIndex = 4
Me.btExit.TabStop = False
Me.btExit.Visible = False
'
'timrPShow
'
Me.timrPShow.Interval = 5000
'
'frmPicShow
'
Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 14.0!)
Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
Me.BackColor = System.Drawing.Color.Black
Me.ClientSize = New System.Drawing.Size(800, 600)
Me.Controls.Add(Me.btPlay)
Me.Controls.Add(Me.btPrev)
Me.Controls.Add(Me.btNext)
Me.Controls.Add(Me.btExit)
Me.Cursor = System.Windows.Forms.Cursors.Default
Me.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
Me.KeyPreview = True
Me.Location = New System.Drawing.Point(125, 107)
Me.Name = "frmPicShow"
Me.RightToLeft = System.Windows.Forms.RightToLeft.No
Me.ShowInTaskbar = False
Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
Me.Text = "Form1"
CType(Me.btPlay, System.ComponentModel.ISupportInitialize).EndInit()
CType(Me.btPrev, System.ComponentModel.ISupportInitialize).EndInit()
CType(Me.btNext, System.ComponentModel.ISupportInitialize).EndInit()
CType(Me.btExit, System.ComponentModel.ISupportInitialize).EndInit()
Me.ResumeLayout(False)
Me.PerformLayout()

End Sub
#End Region
End Class