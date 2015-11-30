Option Strict On
Option Explicit On
Friend Class frmPicSelect
Inherits System.Windows.Forms.Form

Private Sub frmPicSelect_Load _
    (ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) _
     Handles MyBase.Load
  Me.Text = " Picture Show III Version " & My.Application.Info.Version.Major & "." & _
    My.Application.Info.Version.Minor & ":  Select display options"
End Sub
Private Sub cmdChgSourceAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAddDirs.Click
'Add folder(s) to be backed up, and update listbox

  Dim fbdFromSelect As New FolderBrowserDialog()  'pick source folder(s) for backing up
  fbdFromSelect.Description = "Select folders to be backed up"
  fbdFromSelect.RootFolder = Environment.SpecialFolder.MyComputer  'set highest level of tree
  fbdFromSelect.ShowNewFolderButton = False
  fbdFromSelect.SelectedPath = "C:\"                        'set current level of folder tree

  Do
    Dim drFromSelect As DialogResult = fbdFromSelect.ShowDialog()
    If drFromSelect = Windows.Forms.DialogResult.OK Then

      'if entire drive selected...
      If fbdFromSelect.SelectedPath.Length = 3 AndAlso _
        MessageBox.Show("Show images from entire " & fbdFromSelect.SelectedPath & " drive?", _
            "Whole drive selected", MessageBoxButtons.YesNo, _
            MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = _
            Windows.Forms.DialogResult.No Then
        Continue Do   'if not want to back up whole drive, go to next iteration of Do loop
      End If

      lstDirs2Vu.Items.Add(fbdFromSelect.SelectedPath)
      fnBldFileArray(fbdFromSelect.SelectedPath)
      'Debug.Print(">" & fbdFromSelect.SelectedPath & "<")

      If MessageBox.Show("More changes?", "Select To and From Folders", MessageBoxButtons.YesNo, _
          MessageBoxIcon.Question) = Windows.Forms.DialogResult.No Then Exit Do
    Else
      Exit Do   'if user clicked on Cancel, don't ask if more changes
    End If
  Loop

End Sub

Private Sub cmdExit_Click(ByVal eventSender As System.Object, _
    ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
  End 'terminate program
End Sub

Private Sub chkShoFName_CheckStateChanged(ByVal eventSender As System.Object, _
    ByVal eventArgs As System.EventArgs) Handles chkShoFName.CheckStateChanged
  If chkShoFName.CheckState = System.Windows.Forms.CheckState.Checked Then
    chkShoPath.Enabled = True
    chkTrimFName.Enabled = True
  Else
    chkShoPath.Enabled = False
    chkShoPath.CheckState = System.Windows.Forms.CheckState.Unchecked
    chkTrimFName.Enabled = False
  End If
End Sub

Private Sub chkFadeWanted_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) _
    Handles chkFadeWanted.CheckedChanged  'occurs when fade is requested or not by user
  '(Note difference from more complex checked-state-changed event above)
  If chkFadeWanted.Checked = True Then       'if fade enabled then enable transition-time box
    udMinor.Enabled = True
    xLblUDMinDesc.Font = New Font(xLblUDMinDesc.Font, Drawing.FontStyle.Bold)
  Else
    udMinor.Enabled = False
    xLblUDMinDesc.Font = New Font(xLblUDMinDesc.Font, Drawing.FontStyle.Regular)
  End If
End Sub

Private Sub cmdSelOK_Click(ByVal eventSender As System.Object, _
    ByVal eventArgs As System.EventArgs) Handles cmdSelOK.Click
  'Dim intFiles As Integer

  intDisplayInterval = CInt(udMajor.Value)  'set the interval
  boShoFName = chkShoFName.Checked   'display filenames?
  boShoPath = chkShoPath.Checked     'display path?
  boTrimFName = chkTrimFName.Checked  'trim file-name?
  boShoDate = chkShoDate.Checked      'display Exif date?
  boShoComt = chkShoComt.Checked      'display Exif comment?
  boRandom = radioRandom.Checked        'display in random order?
  boDateSort = radioByDate.Checked    'sort file array by date?
  boManual = chkManual.Checked        'run under manual control?
  boFadeBase = chkFadeWanted.Checked  'fade transition wanted
  boFade = boFadeBase     'set immed fade switch from base switch
  intFadeInterval = CInt(udMinor.Value)   'length of fade period

  If boManual AndAlso boFadeBase Then  'don't allow fading in manual control
    boFade = False  'set immed fade switch to false
    MessageBox.Show("Fading disabled while in Manual mode", "PicShow Initialize", _
        MessageBoxButtons.OK, MessageBoxIcon.Warning)
  End If

  If intFileCount = 0 Then       'If 0 files, raise error & end the app
    MsgBox("No image files (JPG, JPEG, GIF, BMP, PNG, TIF, TIFF) found", _
        MsgBoxStyle.Critical, "Find Image Files Failed")
    frmPicShow.Close()   'Close open forms and
    Me.Close()           '  dispose of owned resources.
    End                  'Terminate the application.
  End If

  Me.Hide()
End Sub

Private Sub cmdShoSettgs_Click(ByVal sender As Object, ByVal e As System.EventArgs) _
    Handles cmdShoSettgs.Click
  Dim DevM As New DEVMODE
  Dim boRetn As Boolean 'boolean return code

  boRetn = EnumDisplaySettings(0, ENUM_CURRENT_SETTINGS, DevM)
  MessageBox.Show("Display settings -" & vbCrLf & _
    "  Width: " & CStr(DevM.dmPelsWidth) & vbCrLf & _
    "  Height: " & CStr(DevM.dmPelsHeight) & vbCrLf & _
    "  Refresh rate: " & CStr(DevM.dmDisplayFrequency) & vbCrLf & _
    "  Color depth: " & CStr(DevM.dmBitsPerPel) & " bits/pixel", _
    "Video settings", MessageBoxButtons.OK, MessageBoxIcon.Information)

End Sub

Private Sub chkSubs_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkSubs.CheckedChanged
  If chkSubs.Checked = False Then
    ioSrchOpt = FileIO.SearchOption.SearchTopLevelOnly  'Do not want lower-level folders
  Else
    ioSrchOpt = FileIO.SearchOption.SearchAllSubDirectories  'Show images from all sublevels (default)
  End If
End Sub
End Class