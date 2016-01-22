Imports GalaSoft.MvvmLight.Command
Imports DeckLinkAPI
Imports System.Runtime.InteropServices
Imports Newtonsoft.Json
Imports System.Collections.ObjectModel

Public Class MainWindowViewModel
    Inherits GalaSoft.MvvmLight.ViewModelBase

    Dim m_deckClass As DecklinkClass

    Public Sub New()
        If IsInDesignMode Then
        Else
            m_deckClass = New DecklinkClass
            _Camera1 = New CameraData.Camera(1, m_deckClass)
            SelectedCamara = _Camera1
            GalaSoft.MvvmLight.Threading.DispatcherHelper.Initialize()
        End If
    End Sub

    Public ReadOnly Property ListOfDisplayModes As List(Of DisplayModeViewModel)
        Get
            Dim _returnList As New List(Of DisplayModeViewModel)
            Dim _displayModeIterator As IDeckLinkDisplayModeIterator
            If Not IsNothing(m_deckClass.m_deckLink) Then
                m_deckClass.m_deckLinkOutput.GetDisplayModeIterator(_displayModeIterator)
                Dim _displyMode As IDeckLinkDisplayMode
                Do
                    _displayModeIterator.Next(_displyMode)
                    If Not IsNothing(_displyMode) Then
                        _returnList.Add(New DisplayModeViewModel(_displyMode))
                    Else
                        Exit Do
                    End If
                Loop
            End If
            Return _returnList
        End Get
    End Property

    Dim _SelectedDisplayMode As DisplayModeViewModel = Nothing
    Public Property SelectedDisplayMode As DisplayModeViewModel
        Get
            Return _SelectedDisplayMode
        End Get
        Set(value As DisplayModeViewModel)
            _SelectedDisplayMode = value
            m_deckClass.m_DisplayMode = _SelectedDisplayMode.DisplayMode
            RaisePropertyChanged("SelectedDisplayMode")
        End Set
    End Property

    Public ReadOnly Property StartScheduleFramesCommand As GalaSoft.MvvmLight.Command.RelayCommand
        Get
            Return New GalaSoft.MvvmLight.Command.RelayCommand(AddressOf ScheduleFramesAction)
        End Get
    End Property

    Public Sub ScheduleFramesAction()
        m_deckClass.InitVideo()
    End Sub

    Public ReadOnly Property StartScheduleBarsCommand As GalaSoft.MvvmLight.Command.RelayCommand
        Get
            Return New GalaSoft.MvvmLight.Command.RelayCommand(AddressOf ScheduleBarsAction)
        End Get
    End Property

    Public Sub ScheduleBarsAction()
        Dim m_CameraControl As New CameraData.CcuVideoWhiteBalance(1)
        m_deckClass.CameraControlData.Add(m_CameraControl)
    End Sub

    Public ReadOnly Property ListOfComPorts() As List(Of String)
        Get
            Return System.IO.Ports.SerialPort.GetPortNames().ToList()
        End Get
    End Property

    Private _SelectedComPort As String
    Public Property SelectedComPort() As String
        Get
            Return _SelectedComPort
        End Get
        Set(ByVal value As String)
            _SelectedComPort = value
            RaisePropertyChanged("SelectedComPort")
        End Set
    End Property

#Region " StartTallyRequest Command"

    Public ReadOnly Property StartTallyRequestCommand() As RelayCommand
        Get
            Return New RelayCommand(AddressOf StartTallyRequestAction)
        End Get
    End Property

    Private Sub StartTallyRequestAction()
        If _com.IsOpen Then
            _com.Close()
        End If
        If SelectedComPort <> "" Then
            _com.PortName = SelectedComPort
            _com.BaudRate = 9600
            _com.Parity = IO.Ports.Parity.None
            _com.DataBits = 8
            _com.StopBits = IO.Ports.StopBits.One
            _com.Open()
            _com.Write("$016" & Chr(13))
        End If
    End Sub

    Dim WithEvents _com As New System.IO.Ports.SerialPort
    Dim _comInput As String = ""
    Private Sub _com_DataReceived(sender As Object, e As System.IO.Ports.SerialDataReceivedEventArgs) Handles _com.DataReceived
        _comInput &= _com.ReadExisting()
        If _comInput.Length = 8 Then
            Debug.Print(_comInput)
            Dim _high1 As Integer
            Dim _high2 As Integer
            Dim _low1 As Integer
            Dim _low2 As Integer
            If Integer.TryParse(Mid(_comInput, 2, 1), _high2) Then
                Debug.Print(_high2.ToString())
            End If
            If Integer.TryParse(Mid(_comInput, 3, 1), _high2) Then
                Debug.Print(_high1.ToString())
            End If
            If Integer.TryParse(Mid(_comInput, 4, 1), _low2) Then
                Debug.Print(_low2.ToString())
                GalaSoft.MvvmLight.Threading.DispatcherHelper.RunAsync(New Action(Function()
                                                                                      If (_low2 And 1) = 1 Then TallyCam5 = True Else TallyCam5 = False
                                                                                      If (_low2 And 2) = 2 Then TallyCam6 = True Else TallyCam6 = False
                                                                                      If (_low2 And 4) = 4 Then TallyCam7 = True Else TallyCam7 = False
                                                                                      If (_low2 And 8) = 8 Then TallyCam8 = True Else TallyCam8 = False
                                                                                  End Function))
            End If
            If Integer.TryParse(Mid(_comInput, 5, 1), _low1) Then
                Debug.Print(_low1.ToString())
                GalaSoft.MvvmLight.Threading.DispatcherHelper.RunAsync(New Action(Function()
                                                                                      If (_low1 And 1) = 1 Then TallyCam1 = True Else TallyCam1 = False
                                                                                      If (_low1 And 2) = 2 Then TallyCam2 = True Else TallyCam2 = False
                                                                                      If (_low1 And 4) = 4 Then TallyCam3 = True Else TallyCam3 = False
                                                                                      If (_low1 And 8) = 8 Then TallyCam4 = True Else TallyCam4 = False
                                                                                  End Function))
            End If
            _comInput = ""
            System.Threading.Thread.Sleep(150)
            _com.Write("$016" & Chr(13))
        Else
            If _comInput.Length > 8 Then
                _comInput = ""
                System.Threading.Thread.Sleep(150)
                _com.Write("$016" & Chr(13))
            End If
        End If
    End Sub

#End Region

#Region "Tallys"

    Public Property TallyCam1 As Boolean
        Get
            Return m_deckClass.Tallys(0)
        End Get
        Set(value As Boolean)
            m_deckClass.Tallys(0) = value
            m_deckClass.ScheduleNextFrameBars()
            RaisePropertyChanged("TallyCam1")
        End Set
    End Property

    Public Property TallyCam2 As Boolean
        Get
            Return m_deckClass.Tallys(1)
        End Get
        Set(value As Boolean)
            m_deckClass.Tallys(1) = value
            m_deckClass.ScheduleNextFrameBars()
            RaisePropertyChanged("TallyCam2")
        End Set
    End Property

    Public Property TallyCam3 As Boolean
        Get
            Return m_deckClass.Tallys(2)
        End Get
        Set(value As Boolean)
            m_deckClass.Tallys(2) = value
            m_deckClass.ScheduleNextFrameBars()
            RaisePropertyChanged("TallyCam3")
        End Set
    End Property

    Public Property TallyCam4 As Boolean
        Get
            Return m_deckClass.Tallys(3)
        End Get
        Set(value As Boolean)
            m_deckClass.Tallys(3) = value
            m_deckClass.ScheduleNextFrameBars()
            RaisePropertyChanged("TallyCam4")
        End Set
    End Property

    Public Property TallyCam5 As Boolean
        Get
            Return m_deckClass.Tallys(4)
        End Get
        Set(value As Boolean)
            m_deckClass.Tallys(4) = value
            m_deckClass.ScheduleNextFrameBars()
            RaisePropertyChanged("TallyCam5")
        End Set
    End Property

    Public Property TallyCam6 As Boolean
        Get
            Return m_deckClass.Tallys(5)
        End Get
        Set(value As Boolean)
            m_deckClass.Tallys(5) = value
            m_deckClass.ScheduleNextFrameBars()
            RaisePropertyChanged("TallyCam6")
        End Set
    End Property

    Public Property TallyCam7 As Boolean
        Get
            Return m_deckClass.Tallys(6)
        End Get
        Set(value As Boolean)
            m_deckClass.Tallys(6) = value
            m_deckClass.ScheduleNextFrameBars()
            RaisePropertyChanged("TallyCam7")
        End Set
    End Property

    Public Property TallyCam8 As Boolean
        Get
            Return m_deckClass.Tallys(7)
        End Get
        Set(value As Boolean)
            m_deckClass.Tallys(7) = value
            m_deckClass.ScheduleNextFrameBars()
            RaisePropertyChanged("TallyCam8")
        End Set
    End Property

    Public Property TallyCam9 As Boolean
        Get
            Return m_deckClass.Tallys(8)
        End Get
        Set(value As Boolean)
            m_deckClass.Tallys(8) = value
            m_deckClass.ScheduleNextFrameBars()
            RaisePropertyChanged("TallyCam9")
        End Set
    End Property

    Public Property TallyCam10 As Boolean
        Get
            Return m_deckClass.Tallys(9)
        End Get
        Set(value As Boolean)
            m_deckClass.Tallys(9) = value
            m_deckClass.ScheduleNextFrameBars()
            RaisePropertyChanged("TallyCam10")
        End Set
    End Property

    Public Property TallyCam11 As Boolean
        Get
            Return m_deckClass.Tallys(10)
        End Get
        Set(value As Boolean)
            m_deckClass.Tallys(10) = value
            m_deckClass.ScheduleNextFrameBars()
            RaisePropertyChanged("TallyCam11")
        End Set
    End Property

    Public Property TallyCam12 As Boolean
        Get
            Return m_deckClass.Tallys(11)
        End Get
        Set(value As Boolean)
            m_deckClass.Tallys(11) = value
            m_deckClass.ScheduleNextFrameBars()
            RaisePropertyChanged("TallyCam12")
        End Set
    End Property

    Public Property TallyCam13 As Boolean
        Get
            Return m_deckClass.Tallys(12)
        End Get
        Set(value As Boolean)
            m_deckClass.Tallys(12) = value
            m_deckClass.ScheduleNextFrameBars()
            RaisePropertyChanged("TallyCam13")
        End Set
    End Property

    Public Property TallyCam14 As Boolean
        Get
            Return m_deckClass.Tallys(13)
        End Get
        Set(value As Boolean)
            m_deckClass.Tallys(13) = value
            m_deckClass.ScheduleNextFrameBars()
            RaisePropertyChanged("TallyCam14")
        End Set
    End Property

    Public Property TallyCam15 As Boolean
        Get
            Return m_deckClass.Tallys(14)
        End Get
        Set(value As Boolean)
            m_deckClass.Tallys(14) = value
            m_deckClass.ScheduleNextFrameBars()
            RaisePropertyChanged("TallyCam15")
        End Set
    End Property

    Public Property TallyCam16 As Boolean
        Get
            Return m_deckClass.Tallys(15)
        End Get
        Set(value As Boolean)
            m_deckClass.Tallys(15) = value
            m_deckClass.ScheduleNextFrameBars()
            RaisePropertyChanged("TallyCam16")
        End Set
    End Property

#End Region

#Region " Camera 1 "

    Dim _Camera1 As CameraData.Camera

    Private _SelectedCamara As CameraData.Camera
    Public Property SelectedCamara() As CameraData.Camera
        Get
            Return _SelectedCamara
        End Get
        Set(ByVal inValue As CameraData.Camera)
            _SelectedCamara = inValue
            RaisePropertyChanged("SelectedCamara")
        End Set
    End Property

#End Region

#Region " SaveSettings Command"

    Public ReadOnly Property SaveSettingsCommand() As RelayCommand
        Get
            Return New RelayCommand(AddressOf SaveSettingsAction)
        End Get
    End Property

    Private Sub SaveSettingsAction()
        'TODO: Implement the Command Function
        Dim _textWriter As IO.TextWriter
        'Dim _configFile As New IO.FileInfo(Windows.Forms.Application.StartupPath & IO.Path.DirectorySeparatorChar & "config.txt")
        '_textWriter = _configFile.CreateText()
        '_textWriter.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(SelectedCamara, Formatting.Indented))
        '_textWriter.Close()

        Dim _ButtonConfigFile As New IO.FileInfo(Windows.Forms.Application.StartupPath & IO.Path.DirectorySeparatorChar & "button.txt")
        _textWriter = _ButtonConfigFile.CreateText()
        _textWriter.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(ListOfButtonGroups, Formatting.Indented))
        _textWriter.Close()


    End Sub

#End Region

#Region " LoadSettings Command"

    Public ReadOnly Property LoadSettingsCommand() As RelayCommand
        Get
            Return New RelayCommand(AddressOf LoadSettingsAction)
        End Get
    End Property

    Private Sub LoadSettingsAction()
        Dim _textReader As IO.TextReader
        Dim _configText As String
        'Dim _configFile As New IO.FileInfo(Windows.Forms.Application.StartupPath & IO.Path.DirectorySeparatorChar & "config.txt")
        'If _configFile.Exists Then
        '    _textReader = _configFile.OpenText()
        '    _configText = _textReader.ReadToEnd()
        '    _textReader.Close()
        '    Try
        '        SelectedCamara = Newtonsoft.Json.JsonConvert.DeserializeObject(Of CameraData.Camera)(_configText)
        '    Catch _exception As Exception
        '    End Try
        'End If
        'SelectedCamara.SetDecklinkControl(m_deckClass)

        Dim _ButtonConfigFile As New IO.FileInfo(Windows.Forms.Application.StartupPath & IO.Path.DirectorySeparatorChar & "button.txt")
        If _ButtonConfigFile.Exists Then
            _textReader = _ButtonConfigFile.OpenText()
            _configText = _textReader.ReadToEnd()
            _textReader.Close()
            Try
                ListOfButtonGroups = Newtonsoft.Json.JsonConvert.DeserializeObject(Of ObservableCollection(Of MidiButtonGroup))(_configText)
                For Each _buttonGroup In ListOfButtonGroups
                    For Each _button In _buttonGroup.ListOfButton
                        _button.SendMidiCommand()
                    Next
                Next
            Catch _exception As Exception
            End Try
        End If


    End Sub

#End Region

#Region " Audio "

    Private _ListOfButtonGroups As ObservableCollection(Of MidiButtonGroup)
    Public Property ListOfButtonGroups() As ObservableCollection(Of MidiButtonGroup)
        Get
            If IsNothing(_ListOfButtonGroups) Then
                _ListOfButtonGroups = New ObservableCollection(Of MidiButtonGroup)
                If IsInDesignMode Then
                    _ListOfButtonGroups.Add(New MidiButtonGroup With {.Title = "Monitor"})
                End If
            End If
            Return _ListOfButtonGroups
        End Get
        Set(ByVal inValue As ObservableCollection(Of MidiButtonGroup))
            _ListOfButtonGroups = inValue
            RaisePropertyChanged("ListOfButtonGroups")
        End Set
    End Property

#End Region


End Class

Public Class DisplayModeViewModel

    Public Sub New(displayMode As IDeckLinkDisplayMode)
        Me.DisplayMode = displayMode
    End Sub

    Public Property DisplayMode As IDeckLinkDisplayMode

    Public Overrides Function ToString() As String
        Dim _str As String
        DisplayMode.GetName(_str)
        Return _str
    End Function

End Class