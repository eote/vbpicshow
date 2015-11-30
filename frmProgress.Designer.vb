<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmProgress
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
    Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmProgress))
    Me.ProgressBar1 = New System.Windows.Forms.ProgressBar()
    Me.lblProgress = New System.Windows.Forms.Label()
    Me.cmdCancel = New System.Windows.Forms.Button()
    Me.SuspendLayout()
    '
    'ProgressBar1
    '
    Me.ProgressBar1.Location = New System.Drawing.Point(15, 55)
    Me.ProgressBar1.Name = "ProgressBar1"
    Me.ProgressBar1.Size = New System.Drawing.Size(258, 23)
    Me.ProgressBar1.TabIndex = 0
    '
    'lblProgress
    '
    Me.lblProgress.AutoEllipsis = True
    Me.lblProgress.AutoSize = True
    Me.lblProgress.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
    Me.lblProgress.Location = New System.Drawing.Point(12, 14)
    Me.lblProgress.Name = "lblProgress"
    Me.lblProgress.Size = New System.Drawing.Size(190, 19)
    Me.lblProgress.TabIndex = 1
    Me.lblProgress.Text = "Build-structured-array status"
    '
    'cmdCancel
    '
    Me.cmdCancel.AutoSize = True
    Me.cmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.cmdCancel.Location = New System.Drawing.Point(89, 87)
    Me.cmdCancel.Name = "cmdCancel"
    Me.cmdCancel.Size = New System.Drawing.Size(77, 30)
    Me.cmdCancel.TabIndex = 2
    Me.cmdCancel.Text = "&Cancel"
    Me.cmdCancel.UseVisualStyleBackColor = True
    '
    'frmProgress
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.ClientSize = New System.Drawing.Size(282, 122)
    Me.ControlBox = False
    Me.Controls.Add(Me.cmdCancel)
    Me.Controls.Add(Me.lblProgress)
    Me.Controls.Add(Me.ProgressBar1)
    Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
    Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
    Me.MaximizeBox = False
    Me.MinimizeBox = False
    Me.Name = "frmProgress"
    Me.Text = "Sorting in progress"
    Me.ResumeLayout(False)
    Me.PerformLayout()

End Sub
    Friend WithEvents ProgressBar1 As System.Windows.Forms.ProgressBar
    Friend WithEvents lblProgress As System.Windows.Forms.Label
    Friend WithEvents cmdCancel As System.Windows.Forms.Button
End Class
