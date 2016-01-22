Namespace CameraData
    Public Class CcuAudioPhantomPower
        Inherits CameraControl

        'todo:Audio Phantom Power

        Public Sub New(cameraNumber As Integer)
            Destination = cameraNumber
            Length = 5
            Catergory = 2
            Parameter = 6
            Type = EmType.VoidOrBoolean
            Operation = EmOperation.SetValue
        End Sub

        Public Sub New(cameraNumber As Integer, decklinkControl As DecklinkClass)
            MyClass.New(cameraNumber)
            DecklickControl = decklinkControl
        End Sub

        Protected Overrides ReadOnly Property VancDataValue As List(Of UInteger)
            Get
                If Value = False Then
                    Return GetDataFromInt8(0)
                Else
                    Return GetDataFromInt8(1)
                End If
            End Get
        End Property

        Private _Value As Boolean = False
        Public Property Value() As Boolean
            Get
                Return _Value
            End Get
            Set(ByVal inValue As Boolean)
                _Value = inValue
                RaisePropertyChanged("Value")
                SendValue()
            End Set
        End Property

    End Class
End Namespace