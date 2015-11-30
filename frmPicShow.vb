Option Strict On
Option Explicit On
Imports System.Drawing
Imports System.Drawing.Imaging
Imports SWF = System.Windows.Forms
Imports VB = Microsoft.VisualBasic
Imports System.IO

Friend Class frmPicShow
Inherits System.Windows.Forms.Form
Dim BGContext As BufferedGraphicsContext
Dim BGrafx As BufferedGraphics
Dim LabelFont As Font
Dim intScrHt As Integer 'screen height
Dim intScrWd As Integer 'screen width
Dim fltScrAsp As Single 'screen Width/Ht aspect ratio
Dim intScrSvr As Integer 'is screen saver active? 1=yes, 0=no
Dim intPwrOff As Integer '# of seconds till screen-blank
Dim intRetCod As Integer 'return code from API calls
Dim btLgth As Integer 'height and width of square buttons: in pixels 12/9/06
Dim strFile2Sho As String   'fully qualified name of next file to display; here mostly for debugging
Dim DispTix As Integer   '# of timer ticks in a full display (with or without fade effect)
Dim TickCount As Integer = 0  'count of timer ticks; form-wide scope because re-set at btnPlay-click

Dim aryBitmaps(1) As Bitmap            '2 bitmaps
Dim aryRects(1) As Rectangle           '2 rectangles
'TODO: May want to put bitmaps & rectangles into structure below
Friend Structure PicInfo
  Dim strPicName As String       'picture's filename
  Dim strPicDate As String       'pic's Date Taken
  Dim strPicComment As String    'pic's chosen comment (if any)
End Structure
Dim aryPicInfo(1) As PicInfo
Dim idxCurr As Integer = 0
Dim idxNotCurr As Integer = 1

   'Fade-effect-related
Dim fltOldOpacity As Single = 1.0F  'opacity value of old image; starts at 1, goes down
Dim sngOpacityIncrement As Single   'stepwise change to opacity value
Dim FadeTix As Integer = 0    '# of timer ticks (and phases) in a fade; used to set opacity increment
Dim strFileNext As String     'on-deck fully qualified file path & name

'Dim StpWch As New Stopwatch()   'for development time trials

'-------------------------
Private Declare Function SystemParametersInfo Lib "user32" _
  Alias "SystemParametersInfoA" _
  (ByVal uAction As Integer, ByVal uParam As Integer, _
   ByRef lpvParam As Integer, ByVal fuWinIni As Integer) As Integer

Private Const GETSCREENSAVEACTIVE As Integer = 16
Private Const SETSCREENSAVEACTIVE As Integer = 17

Private Const GETPOWEROFFTIMEOUT As Integer = 80
Private Const SETPOWEROFFTIMEOUT As Integer = 82
'Private Const GETPOWEROFFACTIVE as integer = 84
'Private Const SETPOWEROFFACTIVE As integer = 86

Private Const SENDCHANGE As Integer = 1 'use in fuWinIni parm

'==== screen-resolution API, constants, etc. ====

Private Declare Function ChangeDisplaySettings Lib "user32" _
  Alias "ChangeDisplaySettingsA" _
   (ByRef lpDevMode As DEVMODE, _
    ByVal dwFlags As Integer) _
   As Integer

Enum CDSXRC 'change-display-settings return codes, from winuser.h
  DISP_CHANGE_SUCCESSFUL = 0
  DISP_CHANGE_RESTART = 1
  DISP_CHANGE_FAILED = -1
  DISP_CHANGE_BADMODE = -2
  DISP_CHANGE_NOTUPDATED = -3
  DISP_CHANGE_BADFLAGS = -4
  DISP_CHANGE_BADPARAM = -5
End Enum

Private Sub frmPicShow_Shown( _
    ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) _
    Handles Me.Shown   'was MyBase.Load; changed 9/8/2012

'MyBase behaves like an object variable referring to the base class
'  of the current instance of a class. MyBase is commonly used to access 
'  base class members that are overridden or shadowed in a derived class. 
'Me provides a way to refer to the specific instance of a class or structure 
'  in which the code is currently executing.

  btLgth = btExit.Width 'store local value for sqr button size

  Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
  Me.WindowState = FormWindowState.Maximized  'had to represent a change in state,
    ' else window did not maximize when screen resolution changed (in earlier versions)
  intScrWd = Me.Width   'must appear after code that maximizes window
  intScrHt = Me.Height

  If fnSetSystemArgs() = False Then
    MessageBox.Show("Problem with setting display (screensaver, etc.);" & vbCrLf & _
      " application is terminating")
    subEndMe()
  End If

  Me.SetStyle(ControlStyles.AllPaintingInWmPaint Or _
    ControlStyles.UserPaint Or _
    ControlStyles.OptimizedDoubleBuffer, True)

  'Retrieve Buffered-Graphics Context for current application domain
  BGContext = BufferedGraphicsManager.Current

  ' Set the maximum size for the primary graphics buffer
  ' of the buffered graphics context for the application
  ' domain.  Any allocation requests for a buffer larger 
  ' than this (which shouldn't happen given the size, I think)
  ' will create a temporary buffered graphics 
  ' context to host the graphics buffer.
  ' (This doesn't seem to be necessary, but may aid efficiency)
  BGContext.MaximumBuffer = New Size(Me.Width + 1, Me.Height + 1)

  ' Allocate a graphics buffer the size of this form
  ' using the pixel format of the Graphics created by 
  ' the Form.CreateGraphics() method, which returns a 
  ' Graphics object that matches the pixel format of the form.
  BGrafx = BGContext.Allocate(Me.CreateGraphics(), _
      New Rectangle(0, 0, Me.Width, Me.Height))

  Randomize()     'initialize random-# generator
  timrPShow.Interval = 1000 'set timer interval to one second at start,
                            ' just enough to let taskbar vanish before first image

  DispTix = (intDisplayInterval * 1000) \ conPhaseMsec
  FadeTix = (intFadeInterval * 1000) \ conPhaseMsec      'backslash, to return integer
  If FadeTix > 0 Then sngOpacityIncrement = CSng(1 / FadeTix) 'avoid divide by 0

'Debug.Print("Length of tick:    " & CStr(conPhaseMsec) & " milliseconds")
'Debug.Print("Length of fade:    " & CStr(intFadeInterval) & " secs, or " & CStr(FadeTix) & " ticks")
'Debug.Print("Length of display: " & CStr(intDisplayInterval) & " secs, or " & CStr(DispTix) & " ticks")
'Debug.Print("Length of cycle:   " & CStr(intDisplayInterval + intFadeInterval) & " secs, or " _
'    & CStr(((intDisplayInterval + intFadeInterval) * 1000) \ conPhaseMsec) & " ticks")

  subManualMode(boManual) 'set up for manual or auto mode: show buttons and cursor OR start timer
  'StpWch.Start()    'start the stopwatch 

End Sub

Friend Function fnSetSystemArgs() As Boolean
'Set screen saver off, screen-blank time to very long interval; return True if successful
  Dim fltSizFactor As Single 'resize factor for screen resolution
  Dim intParam As Integer 'get current Sysinfo statuses
  Dim intAPIRetCode As Integer  'success/fail return from APIs; 1 = success
  Const intTimeOut As Integer = 21600 '6 hours, in seconds

  Try
    intAPIRetCode = SystemParametersInfo(GETSCREENSAVEACTIVE, 0, intParam, 0)
    intScrSvr = intParam 'save screen-saver state for later restore

    'Tell system to disable screen saver
    intAPIRetCode = SystemParametersInfo(SETSCREENSAVEACTIVE, 0, intParam, 0)

    'Get, save current screen-blank time
    intAPIRetCode = SystemParametersInfo(GETPOWEROFFTIMEOUT, 0, intParam, 0)
    intPwrOff = intParam 'save screen-blank time

    'Change screen-blank time to 6 hours
    intAPIRetCode = SystemParametersInfo(SETPOWEROFFTIMEOUT, intTimeOut, intParam, SENDCHANGE)
    'intAPIRetCode = SystemParametersInfo(SETPOWEROFFTIMEOUT, intTimeOut, vbNullString, SENDCHANGE)

    'Don't need to turn power-save off if timeout 6 hrs
    'intAPIRetCode = SystemParametersInfo( _
    ''    SETPOWEROFFACTIVE, 0, vbNullString, _
    ''    SENDCHANGE)

    If intAPIRetCode <> 1 Then
      subEndMe() 'terminate app if set-system-args failed
      'NOTE: end-me tries to restore old screen settings
    End If

    fltScrAsp = CSng(intScrWd / intScrHt) 'get screen Width/Ht aspect

    'adjust labels font-size relative to 600x800 screen
    fltSizFactor = CSng(intScrHt / 600)    '1 if 600x800, 2 if 1200x1600
    LabelFont = New Font("Lucida Sans", 9 * fltSizFactor, FontStyle.Regular)

    Return True

  Catch ex As Exception
    MessageBox.Show(ex.Message & vbCrLf & vbCrLf & ex.ToString, "Set system display args", _
      MessageBoxButtons.OK, MessageBoxIcon.Error)
    Return False
  End Try

End Function

Sub subShowPicWithFade()
  Dim ClrMtrx As ColorMatrix = New ColorMatrix           'in System.Drawing.Imaging namespace
  Dim ImgAttrs As ImageAttributes = New ImageAttributes  'in System.Drawing.Imaging namespace
  Dim strFtyp As String     'used to test file-type for Exif validity
  'Debug.Print(Format(StpWch.ElapsedMilliseconds / 1000, "0.000") & " " & "  Show pic with Fade; Old-Opacity: " & fltOldOpacity)

'Try   'TODO restore try-catch after debugging
  BGrafx.Graphics.FillRectangle(Brushes.Black, 0, 0, Me.Width, Me.Height)  'blank the screen

  ClrMtrx.Matrix33 = fltOldOpacity     'set old-image opacity level
  ImgAttrs.SetColorMatrix(ClrMtrx)     'apply opacity level to image attributes
  BGrafx.Graphics.DrawImage(aryBitmaps(idxNotCurr), aryRects(idxNotCurr), 0, 0, _
      aryBitmaps(idxNotCurr).Width, aryBitmaps(idxNotCurr).Height, GraphicsUnit.Pixel, ImgAttrs)

'Debug.Print(Format(StpWch.ElapsedMilliseconds / 1000, "0.000") & ": before set new matrix-33 & img-attrs")
  ClrMtrx.Matrix33 = 1.0F - fltOldOpacity   'set new-image opacity level
  ImgAttrs.SetColorMatrix(ClrMtrx)          'apply opacity level to image attributes
'StpWch.Reset()
'StpWch.Start()
'Debug.Print(Format(StpWch.ElapsedMilliseconds / 1000, "0.000") & ": before draw new image")
  BGrafx.Graphics.DrawImage(aryBitmaps(idxCurr), aryRects(idxCurr), 0, 0, _
      aryBitmaps(idxCurr).Width, aryBitmaps(idxCurr).Height, GraphicsUnit.Pixel, ImgAttrs)
'Debug.Print(Format(StpWch.ElapsedMilliseconds / 1000, "0.000") & ": after draw new image")

  If boShoFName = True Then 'show filename
    subShowName(strFile2Sho)
  End If

  If (boShoDate = True) Or (boShoComt = True) Then
    If strFile2Sho.Length > 4 Then   'look for valid Exif filetype if name long enough
      strFtyp = VB.Right(strFile2Sho, 5)
      If (InStr(strFtyp, ".jpg", CompareMethod.Text) > 0) Or _
         (InStr(strFtyp, ".jpeg", CompareMethod.Text) > 0) Or _
         (InStr(strFtyp, ".tif", CompareMethod.Text) > 0) Or _
         (InStr(strFtyp, ".tiff", CompareMethod.Text) > 0) Then
        subShowExif(idxCurr)      'get Exif data if valid filetype
        '(but the Exif-reader code does not yet return tags from TIFF files)
      End If
    End If
  End If

  BGrafx.Render(Graphics.FromHwnd(Me.Handle))

  Return

'Catch ex As OutOfMemoryException
'  MessageBox.Show(strFile2Sho & vbCrLf & "may not be valid image" & vbCrLf & vbCrLf & ex.ToString, _
'    "Fade-picture Error 1", _
'    MessageBoxButtons.OK, MessageBoxIcon.Error)
'Catch ex As System.IO.FileNotFoundException
'  MessageBox.Show(strFile2Sho & vbCrLf & vbCrLf & ex.Message & vbCrLf & vbCrLf & ex.ToString, _
'    "Fade-picture Error 2", _
'    MessageBoxButtons.OK, MessageBoxIcon.Error)
'Catch ex As ArgumentException
'  MessageBox.Show(strFile2Sho & vbCrLf & vbCrLf & ex.Message & vbCrLf & vbCrLf & ex.ToString, _
'    "Fade-picture Error 3", _
'    MessageBoxButtons.OK, MessageBoxIcon.Error)
'Catch ex As Exception
'  MessageBox.Show(strFile2Sho & vbCrLf & vbCrLf & ex.Message & vbCrLf & vbCrLf & ex.ToString, _
'    "Fade-picture Error 4", _
'    MessageBoxButtons.OK, MessageBoxIcon.Error)
'  Throw ex
'End Try

End Sub

Sub subShowPicNoFade(ByVal strNewFile As String)
  Dim strFtyp As String     'used to test file-type for Exif validity

Try

  BGrafx.Graphics.FillRectangle(Brushes.Black, 0, 0, Me.Width, Me.Height)  'blank the screen

  BGrafx.Graphics.DrawImage(aryBitmaps(idxCurr), aryRects(idxCurr))

  If boShoFName = True Then 'show filename
    subShowName(strNewFile)
  End If

  If (boShoDate = True) Or (boShoComt = True) Then
    If strNewFile.Length > 4 Then   'look for valid Exif filetype if name long enough
      strFtyp = VB.Right(strNewFile, 5)
      If (InStr(strFtyp, ".jpg", CompareMethod.Text) > 0) Or _
         (InStr(strFtyp, ".jpeg", CompareMethod.Text) > 0) Or _
         (InStr(strFtyp, ".tif", CompareMethod.Text) > 0) Or _
         (InStr(strFtyp, ".tiff", CompareMethod.Text) > 0) Then
        subShowExif(idxCurr)      'get Exif data if valid filetype
        '(but the Exif-reader code does not yet return tags from TIFF files)
      End If
    End If
  End If

  BGrafx.Render(Graphics.FromHwnd(Me.Handle))

  Return

Catch ex As OutOfMemoryException
  MessageBox.Show(strNewFile & vbCrLf & "may not be valid image" & vbCrLf & vbCrLf & ex.ToString, _
    "Show-picture Error 1", _
    MessageBoxButtons.OK, MessageBoxIcon.Error)
Catch ex As System.IO.FileNotFoundException
  MessageBox.Show(strNewFile & vbCrLf & vbCrLf & ex.Message & vbCrLf & vbCrLf & ex.ToString, _
    "Show-picture Error 2", _
    MessageBoxButtons.OK, MessageBoxIcon.Error)
Catch ex As ArgumentException
  MessageBox.Show(strNewFile & vbCrLf & vbCrLf & ex.Message & vbCrLf & vbCrLf & ex.ToString, _
    "Show-picture Error 3", _
    MessageBoxButtons.OK, MessageBoxIcon.Error)
Catch ex As Exception
  MessageBox.Show(strNewFile & vbCrLf & vbCrLf & ex.Message & vbCrLf & vbCrLf & ex.ToString, _
    "Show-picture Error 4", _
    MessageBoxButtons.OK, MessageBoxIcon.Error)
  Throw ex
End Try

End Sub

Sub subGetBitmap(ByVal strNewFile As String, _
    ByRef bmpTarget As Bitmap, ByRef rectTarget As Rectangle, ByRef InfoStruc As PicInfo)
'Called from show-pic with and without fade; takes name of image file;
' returns target bitmap, rectangle, and selected metadata
'Sets new bitmap to show and its stretched rectangle; also pic name, date, comment
  Dim fltPicAsp As Single 'image Width/Ht aspect ratio
  Dim fltPicRelWd As Single 'image aspect / screen aspect: > 1 = wider
'Objects used in Date and Exif extraction
  Dim EncodingASCII As System.Text.Encoding = System.Text.Encoding.ASCII
  Dim EncodingUTF8 As System.Text.Encoding = System.Text.Encoding.UTF8
  Dim EncodingUnicode As System.Text.Encoding = System.Text.Encoding.Unicode
  Dim arystrTagNos As String() = {"270", "40092", "37510", "36867"}
  Dim str270 As String = ""      ' x'010E - Image title/description; ASCII
  Dim str40092 As String = ""    ' x'9C9C - Windows Explorer comment
  Dim str37510 As String = ""    ' x'9286 - Exif UserComment: ASCII, Unicode, other,
        ' but delivered by get-string to user as Unicode regardless, I think
  Dim strChosenCmt As String = ""
  Dim strExifDate As String = ""

'Debug.Print(Format(StpWch.ElapsedMilliseconds / 1000, "0.00") & "  sub-GetImage called for: " & strNewFile)

Try
  Dim imgOrig As Image = Nothing    'used to hold orig image from disk before resizing
  If fnChkImageType(strNewFile) Then      'if a recognized image file type,
    imgOrig = Image.FromFile(strNewFile)  '  read image to be resized
  Else
    MessageBox.Show("Do not recognize " & strNewFile, "Show image", MessageBoxButtons.OK, MessageBoxIcon.Error)
    Exit Sub    'should never occur
  End If

'Get Exif values w/o argument exception 
  Dim aryintPropItms() As Integer = imgOrig.PropertyIdList
  Dim boGotExifDateTaken As Boolean = False   'initialize to date-taken Exif tag not-found
  For Each intPropItem As Integer In aryintPropItms

     ' -- Look for Exif date-taken --
   If intPropItem = 36867 Then   ' x'9003' - Exif date/time created
      boGotExifDateTaken = True
    End If

    ' -- Look for Exif comments --
    If intPropItem = 270 Then     ' x'010E' - Image title/description; UTF8, highest prty
      str270 = EncodingUTF8.GetString(imgOrig.GetPropertyItem(270).Value)
    End If

    If intPropItem = 40092 Then   ' x'9C9C' - Windows Explorer comment; Unicode, middle prty
      str40092 = EncodingUnicode.GetString(imgOrig.GetPropertyItem(40092).Value)
    End If

    If intPropItem = 37510 Then   ' x'9286' - Exif User Comment; Unicode, lowest priority
      str37510 = EncodingUnicode.GetString(imgOrig.GetPropertyItem(37510).Value)
    End If

  Next intPropItem

  If boGotExifDateTaken = True Then      'if have exif date-taken tag
    strExifDate = EncodingASCII.GetString(imgOrig.GetPropertyItem(36867).Value)
    strExifDate = VB.Left(strExifDate, 10).Replace(":", "/")     '2010:01:31 -> 2010/01/31
    If VB.IsDate(strExifDate) Then
      strExifDate = Format(CDate(strExifDate), "MMMM d, yyyy")
    Else         'if date not valid, set empty string
      strExifDate = ""
    End If
  Else        'no exif date-taken field, must use file's date-modified property as next best
    Dim fiImgFileInfo As New FileInfo(strNewFile)
    strExifDate = Format(fiImgFileInfo.LastWriteTime, "MMMM d, yyyy")
    'Debug.Print(strNewFile & ": last modified " & strExifDate)
  End If

  'Note: "Option Compare Text" required to make [<> ""] work with Unicode
  If str37510 <> "" Then str37510 = VB.Left(str37510, str37510.Length - 1)
  If str40092 <> "" Then str40092 = VB.Left(str40092, str40092.Length - 1)
  If str270.Length > 0 Then str270 = VB.Left(str270, str270.Length - 1)

  str37510 = Trim(str37510)    'remove blanks at start and end
  str40092 = Trim(str40092)
  str270 = Trim(str270)

  If str270.Length > 0 Then
    strChosenCmt = str270    'highest priority
  ElseIf str40092 <> "" Then
    strChosenCmt = str40092  'medium priority
  ElseIf str37510 <> "" Then
    strChosenCmt = str37510  'lowest priority
  Else
    strChosenCmt = ""        'default
  End If
  InfoStruc.strPicName = strNewFile     'store FQ filename; will trim later if asked
  InfoStruc.strPicComment = strChosenCmt     'store Exif comment in info structure
  InfoStruc.strPicDate = strExifDate         'store date photo taken in info struc

  'Debug.Print(strNewFile)
  'Debug.Print("  Comment: >" & InfoStruc.strPicComment & "<")
  'Debug.Print("  Date: >" & InfoStruc.strPicDate & "<")

  'Determine pic's aspect ratio vs. screen's AR, and handle
  fltPicAsp = CSng(imgOrig.Width / imgOrig.Height)
  fltPicRelWd = CSng(fltPicAsp / (Me.Width / Me.Height))     'if >1, wider than screen

  With rectTarget
    If fltPicRelWd = 1 Then 'if image and screen same shape,
      .Height = Me.Height   ' image height to size of screen
      .Width = Me.Width     ' image width to size of screen
      .Y = 0
      .X = 0
    ElseIf fltPicRelWd < 1 Then  'if image narrower than screen
      .Height = Me.Height   'image height to size of screen
      .Width = CInt((Me.Width * fltPicRelWd)) 'width less as appropriate
      .Y = 0
      .X = CInt((Me.Width - rectTarget.Width) / 2)
    Else 'if image wider than screen
      .Height = CInt((Me.Height / fltPicRelWd)) 'height less than screen height
      .Width = Me.Width   'image width to size of screen
      .Y = CInt((Me.Height - rectTarget.Height) / 2)
      .X = 0
    End If
  End With

  Dim bmpNew As New Bitmap(imgOrig, rectTarget.Width, rectTarget.Height) 'create resized bitmap
  bmpTarget = CType(bmpNew.Clone(), Bitmap)        'return resized bitmap to caller

  If bmpNew IsNot Nothing Then bmpNew.Dispose()
  If imgOrig IsNot Nothing Then imgOrig.Dispose()

  Return

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
    'Stop   'commented out 11/15/2010
Catch ex As Exception
  MessageBox.Show(strNewFile & vbCrLf & vbCrLf & ex.Message & vbCrLf & vbCrLf & ex.ToString, _
    "GetImage Error 4", _
    MessageBoxButtons.OK, MessageBoxIcon.Error)
  Throw ex
End Try

End Sub

Public Sub subShowName(ByVal strFile As String) 'retrieve filename
'TODO: modify/simplify to use filename already retrieved
  Dim strShow As String
  Dim Ix As Integer
  strShow = strFile
  If boShoPath = False Then 'if show filename but not path
    Ix = InStrRev(strFile, "\")
    strShow = VB.Right(strFile, Len(strShow) - Ix)

    'NOTE: filetype is not trimmed off if full path was requested
    Ix = InStrRev(strShow, ".")     'trim period and filetype off
    strShow = VB.Left(strShow, Ix - 1)
  End If

  If boTrimFName Then strShow = fnTrimFName(strShow)

  BGrafx.Graphics.DrawString(strShow, LabelFont, Brushes.Bisque, 4, 8)

  System.Windows.Forms.Application.DoEvents()
End Sub

Private Sub timrPShow_Tick(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) _
    Handles timrPShow.Tick
  Static bo1stTime As Boolean = True
  Static CycleTix As Integer      'if fade used, total ticks of fade plus display
'Dim SysFileInfo As IO.FileInfo

'Debug.Print(Format(StpWch.ElapsedMilliseconds / 1000, "0.00") & "  Tick event entered; count = " & TickCount)

  TickCount += 1         'increment the count by one

  If bo1stTime Then                        'On first entry,
    CycleTix = DispTix + FadeTix       ' set full fade-cycle tick total once
    strFile2Sho = fnGetNextFile(True)    'true => timer call is always forward
  'Debug.Print(Format(StpWch.ElapsedMilliseconds / 1000, "0.00") & "  call 1st getImg to img-new for " & strFile2Sho)
    subGetBitmap(strFile2Sho, aryBitmaps(idxCurr), aryRects(idxCurr), aryPicInfo(idxCurr))   'create image object from file and set its rectangle
  'SysFileInfo = My.Computer.FileSystem.GetFileInfo(strFile2Sho)
  'Debug.Print(Format(StpWch.ElapsedMilliseconds / 1000, "0.00") & "  First file to aryimages(idxcurr); size: " & SysFileInfo.Length & _
  '    "  " & strFile2Sho)
    timrPShow.Interval = conPhaseMsec      ' set timer-tick interval to fade-effect phase length in milliseconds
    subShowPicNoFade(strFile2Sho)        'Display initial image 
    bo1stTime = False
    Exit Sub
  End If

  If boFade = False Then         'if no fade effect
    If TickCount >= DispTix Then  'if have reached end of display time
      TickCount = 0        'reset tick-count
      strFile2Sho = fnGetNextFile(True)    'true => timer call is always forward
    'Debug.Print(Format(StpWch.ElapsedMilliseconds / 1000, "0.00") & "  call later no-fade getImg to img-new for " & strFile2Sho)
      subGetBitmap(strFile2Sho, aryBitmaps(idxCurr), aryRects(idxCurr), aryPicInfo(idxCurr))   'create image object from file and set its rectangle
    'SysFileInfo = My.Computer.FileSystem.GetFileInfo(strFile2Sho)
    'Debug.Print(Format(StpWch.ElapsedMilliseconds / 1000, "0.00") & "  Later no-fade file to aryimages(idxcurr); size: " & SysFileInfo.Length & _
        '"  " & strFile2Sho)
      subShowPicNoFade(strFile2Sho)
      strFile2Sho = ""  'reset file string
    End If

  Else          'If fade effect requested
    If TickCount >= CycleTix Then  'if at start of a new full display
      TickCount = 0        'reset tick-count
      'Stop   'under construction
    ElseIf TickCount >= DispTix Then
      'call fade-handler with fade phase # (starting with 1)
      FadeHandler(TickCount + 1 - DispTix)
    ElseIf TickCount = 2 Then
      'set up next image in sequence; may need to do in separate thread
      strFile2Sho = fnGetNextFile(True)    'to avoid delay, get on-deck image file 
    'Debug.Print(Format(StpWch.ElapsedMilliseconds / 1000, "0.00") & "  call later w/Fade getImg to img-Next for " & strFileNext)
      subGetBitmap(strFile2Sho, aryBitmaps(idxNotCurr), aryRects(idxNotCurr), aryPicInfo(idxNotCurr))   'create image object from file and set its rectangle

    'SysFileInfo = My.Computer.FileSystem.GetFileInfo(strFileNext)
    'Debug.Print(Format(StpWch.ElapsedMilliseconds / 1000, "0.00") & "  Later Fade file to img-Next; size: " & SysFileInfo.Length & _
    '    "  " & strFileNext)
    End If
  End If

End Sub

Private Sub FadeHandler(ByVal intPhase As Integer)
  'Debug.Print(Format(StpWch.ElapsedMilliseconds / 1000, "0.00") & " Fade-handler tick #: " & CStr(intPhase) & "  Old-Opacity: " & fltOldOpacity)

  If intPhase < 2 Then     'If at start of fade, 
    subidxAdvance()        'reverse current and not-current array subscripts
    fltOldOpacity = 1.0F      'Set initial opacity to be decremented.
  End If

  fltOldOpacity = CSng((FadeTix - intPhase) / FadeTix)   'decrement old-opacity
'StpWch.Reset()
'StpWch.Start()
  subShowPicWithFade()
End Sub

Public Function fnRandomPicNo(ByVal intUbnd As Integer) As Integer
  Static aryPicNo() As Boolean 'T/F setting for each pic
  Static boFTSw As Boolean '1st-time switch: false 1st time
  Dim iX As Integer
  Static intLast As Integer '# of last pic, to avoid repeat
  Static intLoopCtr As Integer 'to detect when all pix shown

  If boFTSw = False Then 'if first time into routine,
    boFTSw = True ' change switch and:
    ReDim aryPicNo(intUbnd) ' set array size to # of pix
    aryPicNo(0) = True ' mark first picture as shown
    intLast = 0
    intLoopCtr = 1 ' mark loop ctr for init True
  End If

  Do
    If intLoopCtr >= intUbnd + 1 Then 'if all pix shown,
      For iX = 0 To intUbnd ' mark them all un-shown
        aryPicNo(iX) = False
      Next iX
      intLoopCtr = 0 'reset loop counter
    End If

    iX = CInt((UBound(aryPicNo)) * Rnd()) 'get random pic #
    'iX = CInt((UBound(aryPicNo) + 1) * Rnd()) 'get random pic #

    If aryPicNo(iX) = False Then 'if pic not yet shown,
      If iX <> intLast Then ' and it's not the last one seen
        aryPicNo(iX) = True ' mark it shown
        intLoopCtr = intLoopCtr + 1 ' increment # of pics shown
        intLast = iX ' set # of last pic shown
        fnRandomPicNo = iX ' show this pic
        Exit Do
      End If
    End If

  Loop

End Function

Private Sub frmPicShow_KeyDown(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
  Dim KeyCode As Integer = eventArgs.KeyCode
  Dim Shift As Integer = eventArgs.KeyData \ &H10000

  If boManual = False Then 'if in automatic mode
    If KeyCode = System.Windows.Forms.Keys.M Then   'if m key pressed
      boFade = False            'never fade while in Manual mode
      subManualMode((True))     'enter manual mode
    Else ' if any other key
      subEndMe() '  end the app
    End If
  Else 'if in manual mode
    Select Case KeyCode
      Case System.Windows.Forms.Keys.P ' P: Play (automatic mode)
        btPlay_Click(btPlay, New System.EventArgs())
      Case System.Windows.Forms.Keys.Left ' Left arrow: Previous image
        btPrev_Click(btPrev, New System.EventArgs())
      Case System.Windows.Forms.Keys.Right ' Right arrow: next image
        btNext_Click(btNext, New System.EventArgs())
      Case System.Windows.Forms.Keys.X, Keys.Escape  'X or Escape: exit
        subEndMe() 'restore mouse pointer, terminate the app
      Case System.Windows.Forms.Keys.F1 'F1: display help message
        MsgBox(HelpMsg, MsgBoxStyle.Information, "PicShow key codes and actions") 'mesg is in globals
      'Case Else   'if any other key, [could be risky if clumsy]
      'subEndMe  'restore mouse pointer, terminate the app
    End Select
  End If

End Sub

Private Sub img1_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs)
  frmPicShow_Click(Me, New System.EventArgs()) 'treat mouse-click in image same as in form
End Sub

Private Sub frmPicShow_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Click
  If boManual Then
    btNext_Click(btNext, New System.EventArgs())
  Else
    subEndMe() 'restore mouse pointer, terminate the app
  End If
End Sub

Public Sub subEndMe()
  Dim intParam As Integer
  Windows.Forms.Cursor.Show()

  'Tell system to restore screen saver if used (1 = True)
  intRetCod = SystemParametersInfo(SETSCREENSAVEACTIVE, intScrSvr, intParam, 0)
  '      SETSCREENSAVEACTIVE, 1, intParam, 0)

  'Change screen-blank time back to original amount
  intRetCod = SystemParametersInfo(SETPOWEROFFTIMEOUT, intPwrOff, intParam, SENDCHANGE)
  'intRetCod = SystemParametersInfo(SETPOWEROFFTIMEOUT, intPwrOff, vbNullString, SENDCHANGE)

  ' don't need this with a 6-hour timeout value
  'intRetCod = SystemParametersInfo( _
  '    SETPOWEROFFACTIVE, 1, vbNullString, _
  '      SENDCHANGE)

  Me.Close()     'terminate PicShow
  'End     'terminate PicShow
End Sub

Sub subShowExif(ByVal PixIdx As Integer)    'was myImage As Bitmap)
Dim dateSize As New SizeF    'floating-point rectangle structure for date-label width

  If boShoDate = True Then
    dateSize = BGrafx.Graphics.MeasureString(aryPicInfo(PixIdx).strPicDate, LabelFont)

    BGrafx.Graphics.DrawString(aryPicInfo(PixIdx).strPicDate, LabelFont, Brushes.Bisque, _
        (intScrWd - dateSize.Width - 4), 8)

    System.Windows.Forms.Application.DoEvents()
  End If

  If boShoComt = True Then
    BGrafx.Graphics.DrawString(aryPicInfo(PixIdx).strPicComment, LabelFont, Brushes.Bisque, _
        4, intScrHt - LabelFont.Height - 2)

    System.Windows.Forms.Application.DoEvents()
  End If

End Sub

Private Function fnTrimFName(ByVal FnameIn As String) As String
'Takes original file name (w/o filetype), returns trimmed form
  Dim strTemp As String = FnameIn    'working name
  Dim strPrefix4 As String      '1st 4 characters
  Dim strPrefix2 As String      '1st 2 characters
  Dim strChar As String         'trailing single letter

  If InStr(strTemp, "Panorama", VB.CompareMethod.Text) < 1 Then _
      strTemp = strTemp.Replace("Pano", "") 'trim Pano from filename, but not Panorama 

  strChar = VB.Right(strTemp, 1)   'strip trailing char, if A-D
  Select Case Asc(strChar)
    Case 65 To 68      'A-D uppercase
      strTemp = VB.Left(strTemp, Len(strTemp) - 1)
  End Select

  strPrefix4 = VB.Left(strTemp, 4)   '(if string shorter than 4, returns all bytes)
  Select Case strPrefix4             'follows Option Compare rule (text-compare here)
    Case "APOD"
      If Len(strTemp) > 12 Then
        Dim strDate As String = Mid(strTemp, 9, 2) & "/" & Mid(strTemp, 11, 2) & "/" & Mid(strTemp, 5, 4)
        ' month / day / year
        If IsDate(strDate) Then
          Dim strDtFmtd As String = Format(strDate, "Short Date")
          strTemp = strTemp.Replace(Mid(strTemp, 5, 8), " " & strDtFmtd & " ")
        End If
      End If
      Exit Select
    Case "DSC_"
      strTemp = strTemp.Replace("DSC_", "Nikon DSC_")
      Exit Select
    Case "IMG_"
      strTemp = strTemp.Replace("IMG_", "Canon IMG_")
      Exit Select
    Case "PICT"
      strTemp = strTemp.Replace("PICT", "Minolta PICT")
      Exit Select
    Case "Scan"
      strTemp = strTemp.Replace("Scan", "Scanner Scan")
    Case Else
      '1st char of filename must be uppercase letter, else exit:
      If Asc(VB.Left(strTemp, 1)) < 65 OrElse Asc(VB.Left(strTemp, 1)) > 90 Then Exit Select
      'OK, 1st char is uppercase, let's look at the  first 2 chars:
      strPrefix2 = VB.Left(strTemp, 2)
      Select Case strPrefix2
        Case "AZ", "CA", "CO", "ID", "MT", "NM", "NV", "OR", "UT", "WA", "WY"  'If a state code
        'and "AK", "AL", "AR", "CT", "DE", "FL", "GA", "HI", "IL", "IN", "IA", "KS", "KY", 
        '    "LA", "ME", "MD", "MA", "MI", "MN", "MS", "MO", "NB", "NH", "NJ", "NY", "NC",
        '    "ND", "OH", "OK", "PA", "RI", "SC", "SD", "TN", "TX", "VT", "VA", "WV", "WI"
          'And second char is an uppercase letter:
          If Asc(Mid(strPrefix2, 2, 1)) >= 65 AndAlso Asc(Mid(strPrefix2, 2, 1)) <= 90 Then
          strTemp = strTemp.Insert(2, " ")   'then separate state code from rest of filename
          End If
        Case Else  'if not a state code, might be sequencing letterpair (AA, BB, CC, ... ZZ)
          If Len(strTemp) > 3 AndAlso Asc(VB.Left(strTemp, 1)) = Asc(Mid(strTemp, 2, 1)) Then
            strTemp = VB.Right(strTemp, strTemp.Length - 2)   'trim off 1st 2 chars
          End If
      End Select
  End Select

  'Remove up to 4 leading numeric digits; would hinder ID of displayed photo
  'If Len(strTemp) < 5 Then
  '  fnTrimFName = strTemp
  '  Exit Function
  'End If
  'If IsNumeric(VB.Left(strTemp, 4)) Then
  '  strTemp = VB.Right(strTemp, Len(strTemp) - 4)
  'ElseIf IsNumeric(VB.Left(strTemp, 3)) Then
  '  strTemp = VB.Right(strTemp, Len(strTemp) - 3)
  'ElseIf IsNumeric(VB.Left(strTemp, 2)) Then
  '  strTemp = VB.Right(strTemp, Len(strTemp) - 2)
  'ElseIf IsNumeric(VB.Left(strTemp, 1)) Then
  '  strTemp = VB.Right(strTemp, Len(strTemp) - 1)
  'End If

  strTemp = DTrim(strTemp)   'remove $$ delimited chars

  strTemp = Trim(strTemp)  'remove leading & trailing spaces
  Return strTemp
End Function

Function DTrim(ByVal strIn As String) As String
'remove text between dollar-sign pairs, left to right: abc$xx$de becomes abcde
Dim D1 As Integer            '1-based location of 1st $ in pair
Dim D2 As Integer            '1-based location of 2nd $ in pair
Dim intStart As Integer = 1  '1st char is 1, not 0 for in-string fn
Dim strOut As String

strOut = strIn       'initialize result string

Do
  D1 = InStr(intStart, strOut, "$")
  If D1 > 0 Then
    D2 = InStr(D1 + 1, strOut, "$")
    If D2 > 0 Then
      strOut = strOut.Remove(D1 - 1, D2 - D1 + 1)
    Else
      Exit Do
    End If
  Else
    Exit Do
  End If
Loop

Return strOut
End Function

Public Sub subManualMode(ByVal boManl As Boolean)
  'set app for manual or automatic mode, depending on parm
  Static bo2ndTime As Boolean 'false at 1st run, true after
  Dim BtnSpace As Integer 'space between buttons

  If bo2ndTime = False Then 'initialize button locations
    bo2ndTime = True 'run this part only once in app
    BtnSpace = btLgth 'no separation between buttons
    btExit.Left = intScrWd - (BtnSpace + 2)
    btExit.Top = intScrHt - btLgth - 2
    btNext.Left = intScrWd - ((BtnSpace * 2) + 2)
    btNext.Top = intScrHt - btLgth - 2
    btPrev.Left = intScrWd - ((BtnSpace * 3) + 2)
    btPrev.Top = intScrHt - btLgth - 2
    btPlay.Left = intScrWd - ((BtnSpace * 4) + 2)
    btPlay.Top = intScrHt - btLgth - 2

    If boManl = True Then    'If starting app in Manual mode
      strFile2Sho = fnGetNextFile(True)    'get next file name; true => timer call is always forward
      subGetBitmap(strFile2Sho, aryBitmaps(idxCurr), aryRects(idxCurr), aryPicInfo(idxCurr))   'create image object from file and set its rectangle
      System.Threading.Thread.Sleep(1000)  'Suspend thread while taskbar vanishes
      subShowPicNoFade(strFile2Sho)        'Display initial image 
    End If

  End If

  If boManl = True Then 'if manual control requested
    boManual = True 'set manual switch on
    timrPShow.Enabled = False 'disable timer
    Windows.Forms.Cursor.Show()
    btExit.Visible = True 'all buttons visible
    btExit.Enabled = True '  and enabled
    btNext.Visible = True
    btNext.Enabled = True
    btPrev.Visible = True
    btPrev.Enabled = True
    btPlay.Visible = True
    btPlay.Enabled = True
  Else 'if running on automatic timer,
    boManual = False 'set manual switch off
    timrPShow.Enabled = True 'let timer run
    Windows.Forms.Cursor.Hide()
    btExit.Visible = False 'all buttons hidden
    btExit.Enabled = False '  and disabled
    btNext.Visible = False
    btNext.Enabled = False
    btPrev.Visible = False
    btPrev.Enabled = False
    btPlay.Visible = False
    btPlay.Enabled = False
  End If

End Sub

Private Sub btPlay_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles btPlay.Click
  boFade = boFadeBase    'set immed fade request to original base request when leaving manual mode
  TickCount = 0          'reinitialize timer tick-count when leaving manual control
  Windows.Forms.Cursor.Hide()    'because cursor didn't vanish when start in Manual and then go to Auto mode
  subManualMode(False) 'go to automatic show mode
End Sub

Private Sub btPrev_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles btPrev.Click
'Display previous image; do not use fade effect when in manual mode
  strFile2Sho = fnGetNextFile(False)    'false => go backwards
  subGetBitmap(strFile2Sho, aryBitmaps(idxCurr), aryRects(idxCurr), aryPicInfo(idxCurr))   'create image object from file and set its rectangle
  subShowPicNoFade(strFile2Sho)
  strFile2Sho = ""  'reset file string
End Sub

Private Sub btNext_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles btNext.Click
'Display next image; do not use fade effect when in manual mode
  strFile2Sho = fnGetNextFile(True)    'true if go forward, false if backwards
  subGetBitmap(strFile2Sho, aryBitmaps(idxCurr), aryRects(idxCurr), aryPicInfo(idxCurr))   'create image object from file and set its rectangle
  subShowPicNoFade(strFile2Sho)
  strFile2Sho = ""  'reset file string
End Sub

Private Sub btExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles btExit.Click
  subEndMe() 'restore mouse pointer, terminate the app
End Sub

Public Function fnGetNextFile(ByVal boForward As Boolean) As String
  'Return string of next image file path & name if first parm True, else of previous image
  Static intArraySub As Integer 'curr value of array subscript

  System.Windows.Forms.Application.DoEvents()
  If UBound(aryFiles) = 0 Then Return aryFiles(0) 'if no more than 1 image, no need to loop through array
  If UBound(aryFiles) < 0 Then                    'if no images, error
    MessageBox.Show("Error: no images to display", "Get-next-file", MessageBoxButtons.OK, MessageBoxIcon.Error)
    subEndMe()     'end the application
  End If

  If boForward = True Then
    If boRandom = False Then 'if to show in order
      If intArraySub < UBound(aryFiles) Then 'show next image
        intArraySub = intArraySub + 1
      Else
        intArraySub = 0
      End If
    Else 'if to display randomly
      intArraySub = fnRandomPicNo(UBound(aryFiles))
    End If

  Else 'if going backwards to previous image
    If boRandom = False Then
      If intArraySub > LBound(aryFiles) Then 'if not at start,
        intArraySub = intArraySub - 1 ' show previous;
      Else 'if at first one,
        intArraySub = UBound(aryFiles) 'show final image
      End If
    Else ' Backwards + Random = no-no
      MsgBox("Cannot go backwards in random-order mode", MsgBoxStyle.Exclamation, _
        "PicShow Manual Control")
      Return "000"
    End If
  End If

  Return aryFiles(intArraySub)   'return fully-qualified name of next file to show

End Function

Private Sub subidxAdvance()  'switches indixes for memory stream, image objects, rectangles
'Reverse array indexes for memory stream, image, rectangle arrays
  If idxCurr = 0 Then
    idxCurr = 1
    idxNotCurr = 0
  Else
    idxCurr = 0
    idxNotCurr = 1
  End If
End Sub

End Class