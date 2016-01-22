Imports GalaSoft.MvvmLight.Command
Imports Newtonsoft.Json

<JsonObject(MemberSerialization.OptIn)> _
Public Class MidiButton
    Inherits GalaSoft.MvvmLight.ViewModelBase

#Region " Properties "

    Private _ButtonTitle As String
    <JsonProperty()> _
    Public Property ButtonTitle() As String
        Get
            Return _ButtonTitle
        End Get
        Set(ByVal inValue As String)
            _ButtonTitle = inValue
            RaisePropertyChanged("ButtonTitle")
        End Set
    End Property

    Private _IsOn As Boolean
    <JsonProperty()> _
    Public Property IsOn() As Boolean
        Get
            Return _IsOn
        End Get
        Set(ByVal inValue As Boolean)
            _IsOn = inValue
            RaisePropertyChanged("IsOn")
            SendMidiCommand()
        End Set
    End Property

    <JsonProperty()> _
    Public Property OnCommand As String

    <JsonProperty()> _
    Public Property OffCommand As String

    Private _IsInEditMode As Boolean
    Public Property IsInEditMode() As Boolean
        Get
            Return _IsInEditMode
        End Get
        Set(ByVal inValue As Boolean)
            _IsInEditMode = inValue
            RaisePropertyChanged("IsInEditMode")
        End Set
    End Property

#End Region

#Region " Button Command"

    Public ReadOnly Property ButtonCommand() As RelayCommand
        Get
            Return New RelayCommand(AddressOf ButtonAction)
        End Get
    End Property

    Private Sub ButtonAction()
        'TODO: Implement the Command Function
        IsOn = Not IsOn
    End Sub

    Public Sub SendMidiCommand()
        If IsOn Then
            If OnCommand <> "" Then
                Dim _commands() As String = OnCommand.Split("|")
                For Each _item In _commands
                    MidiClient.GetInstance.Mischpult.Send(_item.Replace("|", ""))
                Next
            End If
        Else
            If OffCommand <> "" Then
                Dim _commands() As String = OffCommand.Split("|")
                For Each _item In _commands
                    MidiClient.GetInstance.Mischpult.Send(_item.Replace("|", ""))
                Next
            End If
        End If
    End Sub

#End Region

#Region " EditOn Command"

    Public ReadOnly Property EditOnCommand() As RelayCommand
        Get
            Return New RelayCommand(AddressOf EditOnAction)
        End Get
    End Property

    Private Sub EditOnAction()
        'TODO: Implement the Command Function
        IsInEditMode = True
    End Sub

#End Region

#Region " EditOff Command"

    Public ReadOnly Property EditOffCommand() As RelayCommand
        Get
            Return New RelayCommand(AddressOf EditOffAction)
        End Get
    End Property

    Private Sub EditOffAction()
        'TODO: Implement the Command Function
        IsInEditMode = False
    End Sub

#End Region


End Class
