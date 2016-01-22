Namespace CameraData
    Public Class CcuColorCorrectionLuma
        Inherits CameraControl

        Public MinValue As Double = 0
        Public MaxValue As Double = 1.0

        Public Sub New(cameraNumber As Integer)
            Destination = cameraNumber
            Length = 6
            Catergory = 8
            Parameter = 5
            Type = EmType.Signed5X11FixedPoint
            Operation = EmOperation.SetValue
        End Sub

        Public Sub New(cameraNumber As Integer, decklinkControl As DecklinkClass)
            MyClass.New(cameraNumber)
            DecklickControl = decklinkControl
        End Sub

        Public ReadOnly Property MinGuiValue() As Double
            Get
                Return MinValue
            End Get
        End Property

        Public ReadOnly Property MaxGuiValue() As Double
            Get
                Return MaxValue
            End Get
        End Property

        Protected Overrides ReadOnly Property VancDataValue As List(Of UInteger)
            Get
                Return GetDataFromFixed16(Value)
            End Get
        End Property

        Private _Value As Double = 1.0
        Public Property Value() As Double
            Get
                Return _Value
            End Get
            Set(ByVal inValue As Double)
                If inValue < MinValue Then inValue = MinValue
                If inValue > MaxValue Then inValue = MaxValue
                inValue = Math.Round(inValue, 3)
                _Value = inValue
                RaisePropertyChanged("Value")
                SendValue()
            End Set
        End Property

    End Class
End Namespace