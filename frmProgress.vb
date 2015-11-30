Public Class frmProgress

Friend Sub ModifyStatus(txtStatus As String)
 lblProgress.Text = txtStatus
End Sub

Private Sub frmProgress_Load(sender As Object, e As System.EventArgs) Handles Me.Load
  Me.Refresh()
End Sub

Private Sub cmdCancel_Click(sender As System.Object, e As System.EventArgs) Handles cmdCancel.Click
  End
End Sub
End Class