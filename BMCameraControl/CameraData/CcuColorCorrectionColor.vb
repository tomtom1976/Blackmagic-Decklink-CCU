Namespace CameraData
    Public Class CcuColorCorrectionColor
        Inherits CameraControl

        Public MinHueValue As Double = -1.0
        Public MaxHueValue As Double = 1.0

        Public MinSatValue As Double = 0
        Public MaxSatValue As Double = 2.0

        Public Sub New()
        End Sub

        Public Sub New(cameraNumber As Integer)
            Destination = cameraNumber
            Length = 8
            Catergory = 8
            Parameter = 6
            Type = EmType.Signed5X11FixedPoint
            Operation = EmOperation.SetValue
        End Sub

        Public Sub New(cameraNumber As Integer, decklinkControl As DecklinkClass)
            MyClass.New(cameraNumber)
            DecklickControl = decklinkControl
        End Sub

        Protected Overrides ReadOnly Property VancDataValue As List(Of UInteger)
            Get
                Dim _return As New List(Of UInteger)
                _return.AddRange(GetDataFromFixed16(HueValue()))
                _return.AddRange(GetDataFromFixed16(SatValue()))
                Return _return
            End Get
        End Property

        Public ReadOnly Property MinHueGuiValue() As Double
            Get
                Return MinHueValue
            End Get
        End Property

        Public ReadOnly Property MaxHueGuiValue() As Double
            Get
                Return MaxHueValue
            End Get
        End Property

        Public ReadOnly Property MinSatGuiValue() As Double
            Get
                Return MinSatValue
            End Get
        End Property

        Public ReadOnly Property MaxSatGuiValue() As Double
            Get
                Return MaxSatValue
            End Get
        End Property

        Private _HueValue As Double = 0.0
        Public Property HueValue() As Double
            Get
                Return _HueValue
            End Get
            Set(ByVal inValue As Double)
                If inValue < MinHueValue Then inValue = MinHueValue
                If inValue > MaxHueValue Then inValue = MaxHueValue
                inValue = Math.Round(inValue, 3)
                _HueValue = inValue
                RaisePropertyChanged("HueValue")
                SendValue()
            End Set
        End Property

        Private _SatValue As Double = 1.0
        Public Property SatValue() As Double
            Get
                Return _SatValue
            End Get
            Set(ByVal inValue As Double)
                If inValue < MinSatValue Then inValue = MinSatValue
                If inValue > MaxSatValue Then inValue = MaxSatValue
                inValue = Math.Round(inValue, 3)
                _SatValue = inValue
                RaisePropertyChanged("SatValue")
                SendValue()
            End Set
        End Property

    End Class
End Namespace