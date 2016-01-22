Namespace CameraData
    Public Class CcuAudioMicLevel
        Inherits CameraControl

        'todo:Audio MicLevel

        Public Sub New(cameraNumber As Integer)
            Destination = cameraNumber
            Length = 6
            Catergory = 2
            Parameter = 0
            Type = EmType.Signed5X11FixedPoint
            Operation = EmOperation.SetValue
        End Sub

        Public Sub New(cameraNumber As Integer, decklinkControl As DecklinkClass)
            MyClass.New(cameraNumber)
            DecklickControl = decklinkControl
        End Sub

        Protected Overrides ReadOnly Property VancDataValue As List(Of UInteger)
            Get
                Return GetDataFromFixed16(Value)
            End Get
        End Property

        Private _Value As Double = 0.5
        Public Property Value(Optional autoSend As Boolean = True) As Double
            Get
                Return _Value
            End Get
            Set(ByVal inValue As Double)
                If inValue < 0 Then inValue = 0
                If inValue > 1 Then inValue = 1
                inValue = Math.Round(inValue, 2)
                _Value = inValue
                RaisePropertyChanged("Value")
                RaisePropertyChanged("ValueUpdate")
                If autoSend Then SendValue()
            End Set
        End Property

        Public Property ValueUpdate() As Double
            Get
                Return CInt(Value * 100)
            End Get
            Set(ByVal inValue As Double)
                Value(True) = inValue / 100
            End Set
        End Property

    End Class
End Namespace