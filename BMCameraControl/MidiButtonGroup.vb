Imports System.Collections.ObjectModel
Imports Newtonsoft.Json
Imports GalaSoft.MvvmLight.Command

<JsonObject(MemberSerialization.OptIn)> _
Public Class MidiButtonGroup
    Inherits GalaSoft.MvvmLight.ViewModelBase

    Private _Title As String
    <JsonProperty()> _
    Public Property Title() As String
        Get
            Return _Title
        End Get
        Set(ByVal inValue As String)
            _Title = inValue
            RaisePropertyChanged("Title")
        End Set
    End Property

    Dim _ListOfButton As ObservableCollection(Of MidiButton)
    <JsonProperty()> _
    Public Property ListOfButton As ObservableCollection(Of MidiButton)
        Get
            If IsNothing(_ListOfButton) Then
                _ListOfButton = New ObservableCollection(Of MidiButton)
                If IsInDesignMode Then
                    _ListOfButton.Add(New MidiButton With {.ButtonTitle = "TEST"})
                End If
            End If
            Return _ListOfButton
        End Get
        Set(value As ObservableCollection(Of MidiButton))
            _ListOfButton = value
        End Set
    End Property

#Region " AddButton Command"

    Public ReadOnly Property AddButtonCommand() As RelayCommand
        Get
            Return New RelayCommand(AddressOf AddButtonAction)
        End Get
    End Property

    Private Sub AddButtonAction()
        'TODO: Implement the Command Function
        _ListOfButton.Add(New MidiButton)
    End Sub

#End Region

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
