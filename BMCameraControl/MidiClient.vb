Public Class MidiClient

#Region " Singelton Pattern "

    ' Variable zur Speicherung der einzigen Instanz.
    Private Shared _Instance As MidiClient = Nothing

    ' Hilfsvariable für eine sichere Threadsynchronisierung.
    Private Shared ReadOnly Mylock As New Object()

    ' Konstruktor ist privat, damit die Klasse nur aus sich selbst heraus instanziiert werden kann.
    Private Sub New()
        'Der Konstruktor. Dieser wird bei beim ersten ersten Aufruf der GetInstance Moethode aufgerufen
        'Initialisierungen sollten in der InitPrivate erfolgen, da diese mit einer Fehlerbehandlung ausgestatet ist.

        InitPrivate()

    End Sub

    ' Diese Shared Methode liefert die einzige Instanz der Klasse zurück.
    Friend Shared Function GetInstance() As MidiClient
        SyncLock (Mylock)
            If _Instance Is Nothing Then
                _Instance = New MidiClient()
            End If
        End SyncLock

        Return _Instance

    End Function

    Private Sub InitPrivate()
        Try
            'Hier sollen alle zur Initialisierung des Wprkspaces stehen.
            Mischpult.PortId = 3
            Mischpult.ClientId = 1
            Mischpult.DisplayValue = "Mischpult"
        Catch _exception As Exception
            'Hier soll die Fehlerbehandlung erfolgen, wenn die Initialisierung der Instanz Fehler bringt
        End Try
    End Sub

#End Region

#Region " Member "

    Public Mischpult As New Devices.MidiDevice

#End Region

End Class
