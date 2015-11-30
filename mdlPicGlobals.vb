Option Strict On
Option Explicit On
Imports System.IO
Imports VB = Microsoft.VisualBasic
Imports WSH = IWshRuntimeLibrary

Friend Structure FNameDate
  Implements IComparable
  Dim FName As String
  Dim FDate As Date

  Function CompareTo(ByVal obj As Object) As Integer _
    Implements IComparable.CompareTo
    Dim other As FNameDate = DirectCast(obj, FNameDate)
    Return System.DateTime.Compare(Me.FDate, other.FDate)
  End Function
End Structure

Module mdlPicGlobals  'for variables and code shared between Select and Show forms
  '  Sample command-line arguments:
  'C:\alldat\pix\edited\astro, interval=3, -showname, -showpath, -random, -showdate, -showcomment, -manual
  'C:\AllDat\Pix\HiQual\CurrentHQ\2006, -showname, interval=4
  'C:\AllDat\Pix\Edited\Tests\PicShoTst\Test01 interval=3 -showname -showdate -showcomment
  'C:\AllDat\Pix\Edited\Tests\PicShoTst\Test02 -manual -fade
  'C:\AllDat\Pix\Edited\Tests\PicShoTst\Test02  interval=5 -showname -showdate -showcomment -random
  'C:\AllDat\Pix\Edited\Tests\PicShoTst\T03md  -showname -fade -interval=8

  Friend intDisplayInterval As Integer = 8   '# of seconds between slides; default value shown

  Friend boShoFName As Boolean = False  'display file name with image?
  Friend boShoPath As Boolean = False   'display file path with name?
  Friend boTrimFName As Boolean = True  'trim the file name if displayed?
  Friend boRandom As Boolean = False    'show pix in random order?
  Friend boDateSort As Boolean = False  'sort pix file array by date? (may still be shown randomly)
  Friend aryFiles(-1) As String   'array of files to display
  Friend strCmdArgs As String     'command parms if any
  Friend boShoDate As Boolean = False  'display file date?
  Friend boShoComt As Boolean = False  'display file Comment?
  Friend boManual As Boolean = False   'under manual control?
  Private dirInfo As IO.DirectoryInfo
  Friend intFileCount As Integer = 0   'cumulative all-folders value; incremented by fn-BldFileArray
  Private intDirCount As Integer = 0
  Friend ioSrchOpt As VB.FileIO.SearchOption = FileIO.SearchOption.SearchAllSubDirectories  'default to all subs
  Friend arystrImgNLinkTypes() As String = {"*.JPG", "*.JPEG", "*.GIF", "*.BMP", "*.PNG", "*.TIF", "*.TIFF", "*.LNK"}
    'initialize with wildcard, dot, and image **and shortcut** filetypes
  'Friend arystrImgTypes() As String = {"*.JPG", "*.JPEG", "*.GIF", "*.BMP", "*.PNG", "*.TIF", "*.TIFF"}
    'just image types, no shortcuts

   'Fade-effect-related
  Friend boFade As Boolean = False   'want gradual fade transition between images?
  Friend boFadeBase As Boolean = False  'user setting (or default) to return to after manual control

  Friend intFadeInterval As Integer = 4  'length in seconds of fade (out + in)
  Friend Const conPhaseMsec As Integer = 100  'milliseconds for each fade phase

  Friend Const HelpMsg As String = "Available keys in manual mode are:" & vbCrLf & vbCrLf & _
    "Left arrow: previous image" & vbCrLf & "Right arrow: next image" & vbCrLf & vbCrLf & _
    "x: terminate the application" & vbCrLf & "p: play in automatic mode" & vbCrLf & _
    "  (return to manual mode by pressing m)" & vbCrLf & vbCrLf & _
    "F1: this help message" & vbCrLf & vbCrLf & _
    "All other keys are ignored in manual," & vbCrLf & _
    "  terminate application in auto mode"

Friend Declare Function EnumDisplaySettings Lib "user32" _
  Alias "EnumDisplaySettingsA" _
  (ByVal lpszDeviceName As Integer, _
   ByVal iModeNum As Integer, _
   ByRef lpDevMode As DEVMODE) As Boolean

Friend Const CCDEVICENAME As Integer = 32
Friend Const CCFORMNAME As Integer = 32
Friend Const DM_PELSWIDTH As Integer = &H80000
Friend Const DM_PELSHEIGHT As Integer = &H100000
Friend Const DM_DISPLAYFREQUENCY As Integer = &H400000
Friend Const CDS_TEST As Integer = &H4
Friend Const ENUM_CURRENT_SETTINGS As Integer = -1

Friend Structure DEVMODE
  <VBFixedString(CCDEVICENAME), _
    System.Runtime.InteropServices.MarshalAs( _
    System.Runtime.InteropServices.UnmanagedType.ByValArray, _
    SizeConst:=CCDEVICENAME)> Public dmDeviceName() As Char
  Dim dmSpecVersion As Short
  Dim dmDriverVersion As Short
  Dim dmSize As Short
  Dim dmDriverExtra As Short
  Dim dmFields As Integer
  Dim dmOrientation As Short
  Dim dmPaperSize As Short
  Dim dmPaperLength As Short
  Dim dmPaperWidth As Short
  Dim dmScale As Short
  Dim dmCopies As Short
  Dim dmDefaultSource As Short
  Dim dmPrintQuality As Short
  Dim dmColor As Short
  Dim dmDuplex As Short
  Dim dmYResolution As Short
  Dim dmTTOption As Short
  Dim dmCollate As Short
  <VBFixedString(CCFORMNAME), _
    System.Runtime.InteropServices.MarshalAs( _
    System.Runtime.InteropServices.UnmanagedType.ByValArray, _
    SizeConst:=CCFORMNAME)> Public dmFormName() As Char
  Dim dmUnusedPadding As Short
  Dim dmBitsPerPel As Short
  Dim dmPelsWidth As Integer
  Dim dmPelsHeight As Integer
  Dim dmDisplayFlags As Integer
  Dim dmDisplayFrequency As Integer
  Dim dmICMMethod As Integer 'NT 4.0
  Dim dmICMIntent As Integer 'NT 4.0
  Dim dmMediaType As Integer 'NT 4.0
  Dim dmDitherType As Integer 'NT 4.0
  Dim dmReserved1 As Integer 'NT 4.0
  Dim dmReserved2 As Integer 'NT 4.0
  Dim dmPanningWidth As Integer 'Win2000
  Dim dmPanningHeight As Integer 'Win2000
End Structure

Friend Sub subParseIntvl(ByVal strIParm As String)
  'parse "interval=nn" command parm, to set length picture is shown
  Dim strIValue As String
  strIValue = VB.Right(strIParm, Len(strIParm) - InStr(strIParm, "="))
  If IsNumeric(strIValue) Then
    If CInt(strIValue) >= frmPicSelect.udMajor.Minimum And CInt(strIValue) <= frmPicSelect.udMajor.Maximum Then
    'If CInt(strIValue) > 3 And CInt(strIValue) < 31 Then
      intDisplayInterval = CShort(strIValue)
    Else
      MsgBox(strIValue & "  is not a valid interval value;  " & vbCrLf & vbCrLf & _
        "Range is 4 - 30", MsgBoxStyle.Critical, "Command-line Parse Error")
      End
    End If
  End If
End Sub

Friend Sub subParseFadeTransit(ByVal strTParm As String)
  'parse "transit=nn" command parm, to set length of fade transition interval
  Dim strTValue As String
  strTValue = VB.Right(strTParm, Len(strTParm) - InStr(strTParm, "="))
  If IsNumeric(strTValue) Then
    If CInt(strTValue) >= frmPicSelect.udMinor.Minimum And CInt(strTValue) <= frmPicSelect.udMinor.Maximum Then
     'If CInt(strTValue) > 1 And CInt(strTValue) < 9 Then
     intFadeInterval = CShort(strTValue)
    Else
      MsgBox(strTValue & "  is not a valid interval value;  " & vbCrLf & vbCrLf & _
        "Range is 2 - 8", MsgBoxStyle.Critical, "Command-line Parse Error")
      End
    End If
  End If
End Sub

Friend Function fnChkImageType _
  (ByVal strFilename As String) As Boolean
'receives arguments: bare or fully qualified filename.filetype
'always returns True if allowed image file 

'TODO  replace this with use of arystrImgTypes filetype array?

Dim strTest As String
strTest = VB.Right(strFilename, strFilename.Length - InStrRev(strFilename, ".", -1, CompareMethod.Text))
'Debug.Print(strFilename & "<   >" & strTest)

  If (InStr(1, strTest, "JPG", CompareMethod.Text) > 0) Or _
      (InStr(1, strTest, "JPEG", CompareMethod.Text) > 0) Or _
      (InStr(1, strTest, "GIF", CompareMethod.Text) > 0) Or _
      (InStr(1, strTest, "BMP", CompareMethod.Text) > 0) Or _
      (InStr(1, strTest, "PNG", CompareMethod.Text) > 0) Or _
      (InStr(1, strTest, "TIF", CompareMethod.Text) > 0) Or _
      (InStr(1, strTest, "TIFF", CompareMethod.Text) > 0) Then
    Return True
  Else
    Return False
  End If
End Function

Friend Function fnBldFileArray(ByVal CurrPath As String) As Integer
  'search path (and by default all subdirs) for image files, store in array
  'returns cumulative all-folders # of files selected (0 is no files, error)

  If Not Directory.Exists(CurrPath) Then
    MessageBox.Show(CurrPath & vbCrLf & "folder does not exist", "Select image files from folder", _
      MessageBoxButtons.OK, MessageBoxIcon.Error)
    Return 0
  End If

  Dim collImgFilesFound As Collections.ObjectModel.ReadOnlyCollection(Of String) = _
      My.Computer.FileSystem.GetFiles(CurrPath, ioSrchOpt, arystrImgNLinkTypes)
  For Each strImgOrLinkFound As String In collImgFilesFound
    Dim FSysInfo As System.IO.FileInfo = My.Computer.FileSystem.GetFileInfo(strImgOrLinkFound)

    If fnChkImageType(FSysInfo.Name) Then
      intFileCount += 1      ' Add one to the file count, if file is an image
      ReDim Preserve aryFiles(aryFiles.GetUpperBound(0) + 1)
      aryFiles(aryFiles.GetUpperBound(0)) = FSysInfo.FullName
    ElseIf (InStr(1, FSysInfo.Name, ".lnk", CompareMethod.Text) > 0) Then   'if a shortcut file:
      Dim MyShell As New WSH.WshShell
      Dim MyShortcut As WSH.WshShortcut
      MyShortcut = (CType(MyShell.CreateShortcut(FSysInfo.FullName), WSH.WshShortcut))
      If My.Computer.FileSystem.FileExists(MyShortcut.TargetPath) _
          AndAlso fnChkImageType(MyShortcut.TargetPath) Then
      'above line tests if target-file of shortcut exists, and if so if it's an image file. If both true, add to array
        intFileCount += 1      ' Add one to the file count, if shortcut points to image file
        ReDim Preserve aryFiles(aryFiles.GetUpperBound(0) + 1)
        aryFiles(aryFiles.GetUpperBound(0)) = MyShortcut.TargetPath
      'Debug.Print("True:   " & FSysInfo.FullName & "   " & MyShortcut.TargetPath)
      End If

      MyShell = Nothing           'clean up objects created above
      MyShortcut = Nothing
    End If

  Next strImgOrLinkFound

  collImgFilesFound = Nothing    'Release the collection after array built

  Return intFileCount
End Function

Function GetImgDate(ByVal strNewFile As String) As Date
'Takes name of image file, sets pic date; either from Exif tag or file-modified property
'Objects used in Date and Exif extraction
  Dim EncodingASCII As System.Text.Encoding = System.Text.Encoding.ASCII
  Dim intDtTakenTagNo As Integer = 36867   '= x'9003' - date/time created
  Dim strExifDate As String = ""
  Dim dateFld As Date = #1/1/1902#  'defaults to midnight January 1 of the year 1902
  GetImgDate = dateFld       'initialize return value to weird default; should always be changed
  Dim boGotExifDateTaken As Boolean = False   'initialize to date-taken Exif tag not-found

Try
  Dim imgOrig As Image = Nothing    'used to hold orig image from disk before resizing
  If fnChkImageType(strNewFile) Then      'if a recognized image file type,
    imgOrig = Image.FromFile(strNewFile)  '  read image to be resized
  Else
    MessageBox.Show("Do not recognize " & strNewFile, "Show image", MessageBoxButtons.OK, MessageBoxIcon.Error)
    Return dateFld   'should never occur
  End If

  'Obtain date picture taken if any, and save
  Dim aryintPropItms() As Integer = imgOrig.PropertyIdList
  For Each intPropItem As Integer In aryintPropItms
    If intPropItem = intDtTakenTagNo Then boGotExifDateTaken = True 'set true if tag x'9003 found
  Next intPropItem

  If boGotExifDateTaken Then
    Dim propItem As Imaging.PropertyItem = imgOrig.GetPropertyItem(intDtTakenTagNo)
    strExifDate = EncodingASCII.GetString(propItem.Value)
    strExifDate = VB.Left(strExifDate, strExifDate.Length - 1)  'delete trailing x'00
    Dim strYYYYMMDD As String = VB.Left(strExifDate, 10).Replace(":", "/")  '2010:01:31 -> 2010/01/31
    strExifDate = strYYYYMMDD & VB.Right(strExifDate, 9)    'YYYY/MM/DD HH:MM:SS  format
    If VB.IsDate(strExifDate) Then
      dateFld = CDate(strExifDate)
      strExifDate = Format(CDate(strExifDate), "MMMM d, yyyy")
    Else         'if date not valid, set empty string
      'dateFld = #1/1/1901#
      strExifDate = ""
    End If
  Else   'no exif date-taken field, must use file's date-modified property as next best
    Dim fiImgFileInfo As New FileInfo(strNewFile)
    dateFld = fiImgFileInfo.LastWriteTime
    'Debug.Print(CStr(dateFld))
  End If

  If imgOrig IsNot Nothing Then imgOrig.Dispose()

  Return dateFld       'photo date or initial default value

Catch ex As OutOfMemoryException
  MessageBox.Show(strNewFile & vbCrLf & "may not be valid image" & vbCrLf & vbCrLf & ex.ToString, _
    "GetImage Error 1", _
    MessageBoxButtons.OK, MessageBoxIcon.Error)
Catch ex As System.IO.FileNotFoundException
  MessageBox.Show(strNewFile & vbCrLf & vbCrLf & ex.Message & vbCrLf & vbCrLf & ex.ToString, _
    "GetImage Error 2", _
    MessageBoxButtons.OK, MessageBoxIcon.Error)
Catch ex As ArgumentException
  MessageBox.Show(strNewFile & vbCrLf & vbCrLf & ex.Message & vbCrLf & vbCrLf & ex.ToString, _
    "GetImage Error 3", _
    MessageBoxButtons.OK, MessageBoxIcon.Error)
Catch ex As Exception
  MessageBox.Show(strNewFile & vbCrLf & vbCrLf & ex.Message & vbCrLf & vbCrLf & ex.ToString, _
    "GetImage Error 4", _
    MessageBoxButtons.OK, MessageBoxIcon.Error)
  Throw ex
End Try

End Function

Friend Sub subSortByDate()
'Build array of FQFN strings and date-time values from ary-files;
' sort the new array based on comparer interface;
' replace ary-files array with sorted elements

  Dim aryNameDate(-1) As FNameDate    'declare empty array of name-date strucs
  Dim boShoProgress As Boolean = False   'only show progress if more than NN files

  If aryFiles.Length >= 100 Then boShoProgress = True

  If aryFiles.Length < 2 Then   'If none or 1 entry, don't sort
    If aryFiles.Length < 1 Then
      MsgBox("No files in source array", MsgBoxStyle.Information, "File sort")
    Else
      MsgBox("Only one file in source array", MsgBoxStyle.Information, "File sort")
    End If
    Exit Sub
  End If

  If boShoProgress Then
    frmProgress.Show()
    frmProgress.ProgressBar1.Maximum = intFileCount
    frmProgress.ProgressBar1.Value = 0  'initialize progress-bar value (not really necessary)
    frmProgress.lblProgress.Text = "Reading dates for " & CStr(intFileCount) & " files"
    frmProgress.Refresh()
  End If

  For Each fn As String In aryFiles
    ReDim Preserve aryNameDate(aryNameDate.GetUpperBound(0) + 1)
    aryNameDate(aryNameDate.GetUpperBound(0)).FName = fn              'add FQFN to new array
    'Obtain the date-taken or date-modified for the file, and store in structure
    aryNameDate(aryNameDate.GetUpperBound(0)).FDate = GetImgDate(fn)  'add date/time to new array
    If boShoProgress Then
      frmProgress.ProgressBar1.Value += 1
      frmProgress.lblProgress.Text = "Done " & CStr(frmProgress.ProgressBar1.Value) & _
          " out of " & CStr(intFileCount) & " files"
    End If
    Application.DoEvents()
  Next fn


  If boShoProgress Then frmProgress.Hide()

  Array.Sort(aryNameDate)

  If aryFiles.Length = aryNameDate.Length Then
    For Ix As Integer = 0 To aryFiles.Length - 1
      aryFiles(Ix) = aryNameDate(Ix).FName
    Next Ix
  Else   'array-length mismatch; serious error
    MsgBox("Error:" & vbCrLf & "Files array count: " & aryFiles.Length & vbCrLf & _
        "Sorted array count: " & aryNameDate.Length, MsgBoxStyle.Critical, "Array-length mismatch")
  End If

End Sub

End Module