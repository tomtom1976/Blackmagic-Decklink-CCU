Imports DeckLinkAPI
Imports System.Runtime.InteropServices

Public Class DecklinkClass

    Private m_running As Boolean = False
    Public m_deckLink As IDeckLink
    Public m_deckLinkOutput As IDeckLinkOutput

    Public m_DisplayMode As IDeckLinkDisplayMode

    Private m_frameWidth As Integer
    Private m_frameHeight As Integer
    Private m_frameDuration As Long
    Private m_frameTimescale As Long
    Private m_framesPerSecond As Integer
    Private m_videoFrameBlack As IDeckLinkMutableVideoFrame
    Private m_videoFrameBars As IDeckLinkMutableVideoFrame
    Private m_videoFrameBarsYUV As IDeckLinkMutableVideoFrame
    Private m_totalFramesScheduled As Integer

    Private m_ancillaryData As New List(Of UInt32)
    Private m_ancillaryDataRaw As New List(Of UInt32)

    Public WithEvents CameraControlData As New ObjectModel.ObservableCollection(Of CameraData.CameraControl)

    Public Tallys(15) As Boolean

    Public Sub New()
        m_running = False

        Dim deckLinkIterator As IDeckLinkIterator = New CDeckLinkIterator()

        deckLinkIterator.Next(m_deckLink)

        m_deckLinkOutput = CType(m_deckLink, IDeckLinkOutput)
    End Sub

    Public Sub InitVideo()
        Dim videoDisplayMode As IDeckLinkDisplayMode
        videoDisplayMode = m_DisplayMode
        m_frameWidth = videoDisplayMode.GetWidth()
        m_frameHeight = videoDisplayMode.GetHeight()
        videoDisplayMode.GetFrameRate(m_frameDuration, m_frameTimescale)

        'Calculate the number of frames per second, rounded up to the nearest integer.  For example, for NTSC (29.97 FPS), framesPerSecond == 30.
        m_framesPerSecond = ((m_frameTimescale + (m_frameDuration - 1)) / m_frameDuration)

        'Set the video output mode
        m_deckLinkOutput.EnableVideoOutput(videoDisplayMode.GetDisplayMode(), _BMDVideoOutputFlags.bmdVideoOutputVANC)

        'Generate a frame of black
        m_deckLinkOutput.CreateVideoFrame(m_frameWidth, m_frameHeight, m_frameWidth * 2, _BMDPixelFormat.bmdFormat8BitYUV, _BMDFrameFlags.bmdFrameFlagDefault, m_videoFrameBlack)
        FillBlack(m_videoFrameBlack)

        'Generate a frame of colour bars
        m_deckLinkOutput.CreateVideoFrame(m_frameWidth, m_frameHeight, m_frameWidth * 2, _BMDPixelFormat.bmdFormat8BitYUV, _BMDFrameFlags.bmdFrameFlagDefault, m_videoFrameBars)
        FillColourBars(m_videoFrameBars)

        'Generate a frame of colour bars
        m_deckLinkOutput.CreateVideoFrame(m_frameWidth, m_frameHeight, 5120, _BMDPixelFormat.bmdFormat10BitYUV, _BMDFrameFlags.bmdFrameFlagDefault, m_videoFrameBarsYUV)

        m_totalFramesScheduled = 0

        m_running = True

        ScheduleNextFrameBars()

    End Sub

    Public Sub ScheduleNextFrameBars()
        If m_running Then
            Dim _converter As New CDeckLinkVideoConversion
            While CameraControlData.Count > 0
                _converter.ConvertFrame(m_videoFrameBars, m_videoFrameBarsYUV)
                WriteAncillaryData(m_videoFrameBarsYUV)
                m_deckLinkOutput.DisplayVideoFrameSync(m_videoFrameBarsYUV)
                System.Windows.Forms.Application.DoEvents()
            End While

            _converter.ConvertFrame(m_videoFrameBars, m_videoFrameBarsYUV)
            WriteAncillaryData(m_videoFrameBarsYUV)
            m_deckLinkOutput.DisplayVideoFrameSync(m_videoFrameBarsYUV)

            GC.Collect()
        End If
    End Sub


    Private Sub FillBlack(theFrame As IDeckLinkVideoFrame)

        Dim buffer As IntPtr
        Dim width As Integer
        Dim height As Integer
        Dim wordsRemaining As Integer
        Dim black As Integer = &H10801080
        Dim index As Integer = 0

        theFrame.GetBytes(buffer)
        width = theFrame.GetWidth()
        height = theFrame.GetHeight()

        wordsRemaining = (width * 2 * height) / 4

        While (wordsRemaining > 0)
            Marshal.WriteInt32(buffer, index * 4, black)
            index += 1
            wordsRemaining -= 1
        End While
    End Sub

    Private Sub FillColourBars(theFrame As IDeckLinkMutableVideoFrame)
        Dim buffer As IntPtr
        Dim width As Integer
        Dim height As Integer
        Dim bars() As Integer = {&HEA80EA80, &HD292D210, &HA910A9A5, &H90229035, &H6ADD6ACA, &H51EF515A, &H286D28EF, &H10801080}
        Dim index As Integer = 0

        theFrame.GetBytes(buffer)
        width = theFrame.GetWidth()
        height = theFrame.GetHeight()

        For y As UInteger = 0 To height - 1
            For x As UInteger = 0 To width - 1 Step 2
                'Write directly into unmanaged buffer
                Marshal.WriteInt32(buffer, index * 4, bars(Fix((x / width) * 8)))
                index += 1
            Next
        Next

    End Sub

    Private Sub WriteAncillaryData(theFrame As IDeckLinkMutableVideoFrame)
        theFrame.SetAncillaryData(GetAncillaryData)
    End Sub

    Private Function GetAncillaryData() As IDeckLinkVideoFrameAncillary

        ' als erstes und das immer schreiben sind die TallyDaten
        Dim _ancillary As IDeckLinkVideoFrameAncillary
        m_deckLinkOutput.CreateAncillaryData(_BMDPixelFormat.bmdFormat10BitYUV, _ancillary)

        Dim _tallyData As List(Of UInt32) = GetAncillaryTallyData()

        Dim _buffer As IntPtr
        _ancillary.GetBufferForVerticalBlankingLine(15, _buffer)
        Dim _bufferPos As Integer = 0
        Dim _writePos As Integer = 0
        For _bufferPos = 0 To _tallyData.Count - 1
            WriteAncDataToLuma(_buffer, _tallyData(_bufferPos), _bufferPos, _writePos)
        Next

        ' jetzt folgen die CameraControlDaten
        If CameraControlData.Count > 0 Then
            Dim _CcuData As List(Of UInt32) = GetAncillaryCameraControlData()

            Dim _bufferCamControl As IntPtr
            _ancillary.GetBufferForVerticalBlankingLine(16, _bufferCamControl)
            _bufferPos = 0
            _writePos = 0
            For _bufferPos = 0 To _CcuData.Count - 1
                WriteAncDataToLuma(_bufferCamControl, _CcuData(_bufferPos), _bufferPos, _writePos)
            Next

        End If

        Return _ancillary

    End Function

    Private Function GetAncillaryTallyData() As List(Of UInt32)
        Dim _tallyData As New List(Of UInt32)
        Dim _tallyDataRaw As New List(Of UInt32)

        _tallyData.Add(0)
        _tallyData.Add(&H3FF)
        _tallyData.Add(&H3FF)
        _tallyData.Add(&H51)
        _tallyData.Add(&H52)
        _tallyData.Add(&H0)
        _tallyData.Add(&H0)

        _tallyData.Add(CUInt((CInt(Tallys(0)) * -1) Or ((CInt(Tallys(1)) * -1) << 4)))
        _tallyData.Add(CUInt((CInt(Tallys(2)) * -1) Or ((CInt(Tallys(3)) * -1) << 4)))
        _tallyData.Add(CUInt((CInt(Tallys(4)) * -1) Or ((CInt(Tallys(5)) * -1) << 4)))
        _tallyData.Add(CUInt((CInt(Tallys(6)) * -1) Or ((CInt(Tallys(7)) * -1) << 4)))
        _tallyData.Add(CUInt((CInt(Tallys(8)) * -1) Or ((CInt(Tallys(9)) * -1) << 4)))
        _tallyData.Add(CUInt((CInt(Tallys(10)) * -1) Or ((CInt(Tallys(11)) * -1) << 4)))
        _tallyData.Add(CUInt((CInt(Tallys(12)) * -1) Or ((CInt(Tallys(13)) * -1) << 4)))
        _tallyData.Add(CUInt((CInt(Tallys(14)) * -1) Or ((CInt(Tallys(15)) * -1) << 4)))

        _tallyData(5) = _tallyData.Count - 6
        For i As Integer = 0 To _tallyData.Count - 1
            If i < 3 Then
                _tallyDataRaw.Add(_tallyData(i))
            Else
                _tallyDataRaw.Add(EncodeByte(_tallyData(i)))
            End If
        Next
        _tallyDataRaw.Add(CalcCheckSum(_tallyDataRaw))

        Return _tallyDataRaw

    End Function

    Private Function GetAncillaryCameraControlData() As List(Of UInt32)
        Dim _CcuData As New List(Of UInt32)
        Dim _CcuDataRaw As New List(Of UInt32)

        _CcuData.Add(0)
        _CcuData.Add(&H3FF)
        _CcuData.Add(&H3FF)
        _CcuData.Add(&H51)
        _CcuData.Add(&H53)
        _CcuData.Add(&H0)

        Dim _Counter As Integer = 0

        While CameraControlData.Count > 0 And _Counter < 31
            _CcuData.AddRange(CameraControlData(0).VancData)
            CameraControlData.RemoveAt(0)
        End While


        _CcuData(5) = _CcuData.Count - 6
        For i As Integer = 0 To _CcuData.Count - 1
            If i < 3 Then
                _CcuDataRaw.Add(_CcuData(i))
            Else
                _CcuDataRaw.Add(EncodeByte(_CcuData(i)))
                'Debug.Print(Convert.ToString(_CcuData(i), 16).PadLeft(2, "0"))
            End If
        Next
        _CcuDataRaw.Add(CalcCheckSum(_CcuDataRaw))

        Return _CcuDataRaw
    End Function

#Region " Funktionen zum Erstellen der VANC Raw-Daten "

    Private Function EncodeByte(dataByte As UInt32) As UInt32
        Dim _temp As UInt32 = dataByte
        _temp = _temp Xor _temp >> 4
        _temp = _temp Xor _temp >> 2
        _temp = _temp Xor _temp >> 1
        _temp = _temp And 1
        dataByte = dataByte Or _temp << 8
        dataByte = dataByte Or ((Not _temp) And 1) << 9
        Return dataByte
    End Function

    Private Sub WriteAncDataToLuma(ByRef buffer As IntPtr, value As UInt32, dataPosition As UInteger, ByRef streamPosition As Integer)
        Select Case dataPosition Mod 3
            Case 0
                value <<= 10
                Marshal.WriteInt32(buffer, streamPosition * 4, CInt(value))
                streamPosition += 1
            Case 1
                Marshal.WriteInt32(buffer, streamPosition * 4, CInt(value))
            Case 2
                value <<= 20
                Marshal.WriteInt32(buffer, streamPosition * 4, Marshal.ReadInt32(buffer, streamPosition * 4) Or CInt(value))
                streamPosition += 1
        End Select
    End Sub

    Private Function CalcCheckSum(data As List(Of UInt32)) As UInteger
        Dim sum As Integer = 0

        For i = 3 To data.Count - 1
            sum = sum + (data(i) And &H1FF)
        Next

        sum = sum And &H1FF
        sum = sum Or ((Not (sum << 1)) And &H200)

        Return sum
    End Function

#End Region

    Private Sub CameraControlData_CollectionChanged(sender As Object, e As System.Collections.Specialized.NotifyCollectionChangedEventArgs) Handles CameraControlData.CollectionChanged
        If e.Action = Specialized.NotifyCollectionChangedAction.Add Then
            ScheduleNextFrameBars()
        End If
    End Sub

    Public ReadOnly Property IsRunning() As Boolean
        Get
            Return m_running
        End Get
    End Property


End Class
