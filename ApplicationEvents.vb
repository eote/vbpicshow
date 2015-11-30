Option Strict On
Option Explicit On
Imports VB = Microsoft.VisualBasic
Namespace My
' The following events are availble for MyApplication:
' Startup: Raised when the application starts, before the startup form is created.
' Shutdown: Raised after all application forms are closed.
'   This event is not raised if the application terminates abnormally.
' UnhandledException: Raised if the application encounters an unhandled exception.
' StartupNextInstance: Raised when launching a single-instance application
'   and the application is already active. 
' NetworkAvailabilityChanged: Raised when network connection is connected or disconnected

Partial Friend Class MyApplication

Private Sub MyApplication_Startup(ByVal sender As Object, _
    ByVal e As Microsoft.VisualBasic.ApplicationServices.StartupEventArgs) _
    Handles Me.Startup

  If e.CommandLine.Count < 1 Then
    frmPicSelect.ShowDialog() 'show selection window if no command-line args
  Else
    For Each cmdArg As String In e.CommandLine  'or My.Application.CommandLineArgs
      If InStr(1, VB.Right(cmdArg, 8), ".picshow", CompareMethod.Text) > 0 Then
        If My.Computer.FileSystem.FileExists(cmdArg) Then
          Dim PSInfo As System.IO.FileInfo
          PSInfo = My.Computer.FileSystem.GetFileInfo(cmdArg)
          If PSInfo.Length > 0 Then      'info file must contain data
            Dim strPSFileText As String     'define string to hold parm text
            Dim IxCrLf As Integer           'define locn where cr-lf found in string
            strPSFileText = My.Computer.FileSystem.ReadAllText(cmdArg)
            Do
              IxCrLf = InStr(strPSFileText, vbCrLf)   'look for cr-lf chars
              If IxCrLf = 0 Then Exit Do 'if not found, leave loop
              strPSFileText = Left(strPSFileText, IxCrLf - 1) & " " & _
                  Right(strPSFileText, strPSFileText.Length - IxCrLf - 1)
            Loop
            'Debug.Print(strPSFileText)
            Dim aryFileParms() As String = Split(strPSFileText)
            For Each strParm As String In aryFileParms
              If strParm <> "" Then subParseArg(strParm)
            Next strParm
          Else          'if length of file is 0 bytes, error
            MessageBox.Show("File " & cmdArg & vbCrLf & vbCrLf & _
              " is empty (0 bytes long)", "Read PicShow Info file", _
              MessageBoxButtons.OK, MessageBoxIcon.Error)
            End
          End If
        Else            'if no such file, error
          MessageBox.Show("File " & cmdArg & vbCrLf & vbCrLf & _
            " does not exist", "Read PicShow Info file", _
            MessageBoxButtons.OK, MessageBoxIcon.Error)
          End
        End If   'end of picshow file exists test
        Exit For    'only one command file is allowed

      Else       'if inline args, not parm-file name
        subParseArg(cmdArg)    'pass arg to parser
      End If
    Next

    If boManual AndAlso boFadeBase Then  'don't allow fading in manual control
      boFade = False  'set immed fade switch to false
      MessageBox.Show("Fading disabled while in Manual mode", "PicShow Initialize", _
          MessageBoxButtons.OK, MessageBoxIcon.Warning)
    End If

  End If  'end test for command-line args or not

  If boDateSort AndAlso boRandom Then
    MessageBox.Show("Sort-by-date and random ordering both selected;" & vbCrLf & _
        "images will be displayed randomly", "Picshow conflicting options chosen", _
        MessageBoxButtons.OK, MessageBoxIcon.Warning)
  End If

  If boDateSort = True Then subSortByDate() 'sort ary-files into date order

  'if fade wanted, and fade length > 100% display, issue notice (commented out for now).
  'TODO Need to think whether this is useful.

  'If boFadeBase AndAlso (intDisplayInterval < intFadeInterval) Then
  '  MessageBox.Show("Length of image display is " & CStr(intDisplayInterval) & " seconds;" & vbCrLf & _
  '      "Length of transition between them is " & CStr(intFadeInterval), _
  '      "PicShow Startup", MessageBoxButtons.OK, MessageBoxIcon.Information)
  'End If

End Sub

Sub subParseArg(ByVal PSArg As String)  'parse a single pic-show startup parm
  'Debug.Print(">" & PSArg & "<")
  Static intFiles As Integer     '# of files found in path, mostly to know for testing
  If VB.Right(PSArg, 1) = "," Then PSArg = VB.Left(PSArg, PSArg.Length - 1)
  If InStr(1, PSArg, "-showname", CompareMethod.Text) > 0 Then boShoFName = True 'show filenames
  If InStr(1, PSArg, "-showpath", CompareMethod.Text) > 0 Then boShoPath = True 'show file path
  If InStr(1, PSArg, "-notrim", CompareMethod.Text) > 0 Then boTrimFName = False 'do not trim file name
  If InStr(1, PSArg, "-showdate", CompareMethod.Text) > 0 Then boShoDate = True 'show filedates
  If InStr(1, PSArg, "-showcomment", CompareMethod.Text) > 0 Then boShoComt = True 'show comments
  If InStr(1, PSArg, "-random", CompareMethod.Text) > 0 Then boRandom = True 'use random order
  If InStr(1, PSArg, "-datesort", CompareMethod.Text) > 0 Then boDateSort = True 'sort file array by date
  If InStr(1, PSArg, "-manual", CompareMethod.Text) > 0 Then boManual = True 'use manual control
  If InStr(1, PSArg, "-nosubs", CompareMethod.Text) > 0 Then ioSrchOpt = FileIO.SearchOption.SearchTopLevelOnly
  If InStr(1, PSArg, "interval=", CompareMethod.Text) > 0 Then subParseIntvl((PSArg))
  If InStr(1, PSArg, "-fade", CompareMethod.Text) > 0 Then
      boFade = True       'want to fade between images
      boFadeBase = True
  End If
  If InStr(1, PSArg, "transit=", CompareMethod.Text) > 0 Then subParseFadeTransit(PSArg)

  If InStr(PSArg, "\") > 0 Then            'If argument is a FQ folder path
    If Not IO.Directory.Exists(PSArg) Then   'If folder path does not exist, error message & terminate app
      MessageBox.Show(PSArg & vbCrLf & "folder does not exist;" & vbCrLf & "terminating PicShow application", _
          "Select image files from folder", _
          MessageBoxButtons.OK, MessageBoxIcon.Error)
      End
    End If
    intFiles = intFiles + fnBldFileArray(PSArg)   'Arg is a valid existing folder; find image files in it
    If intFiles = 0 Then 'If 0 files in folder, notify; other folders may have some
      MessageBox.Show("No image files (JPG, JPEG, GIF, BMP, PNG, TIF, TIFF) found in" _
          & vbCrLf & vbCrLf & PSArg, _
          "Find Image Files", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End If
  End If

  If intFiles = 0 Then   'if no image files found at all, issue message and end application
    MessageBox.Show("No image files (JPG, JPEG, GIF, BMP, PNG, TIF, TIFF) found", _
        "Find Image Files Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
    End
  End If

End Sub

Private Sub MyApplication_UnhandledException( _
    ByVal sender As Object, _
    ByVal e As Microsoft.VisualBasic.ApplicationServices.UnhandledExceptionEventArgs) _
    Handles Me.UnhandledException

  MessageBox.Show("Unhandled exception:" & vbCrLf & e.ToString & vbCrLf & _
    "Sender: " & sender.ToString, "PicShow Application", _
    MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
End Sub
End Class

End Namespace

