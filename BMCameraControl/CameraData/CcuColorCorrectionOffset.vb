Imports GalaSoft.MvvmLight.Command

Namespace CameraData
    Public Class CcuColorCorrectionOffset
        Inherits CameraControl

        Public MinValue As Double = -8.0
        Public MaxValue As Double = 8.0

        Public Sub New(cameraNumber As Integer)
            Destination = cameraNumber
            Length = 12
            Catergory = 8
            Parameter = 3
            Type = EmType.Signed5X11FixedPoint
            Operation = EmOperation.SetValue
        End Sub

        Public Sub New(cameraNumber As Integer, decklinkControl As DecklinkClass)
            MyClass.New(cameraNumber)
            DecklickControl = decklinkControl
        End Sub

        Protected Overrides ReadOnly Property VancDataValue As System.Collections.Generic.List(Of UInteger)
            Get
                Dim _return As New List(Of UInteger)
                _return.AddRange(GetDataFromFixed16(ValueRed))
                _return.AddRange(GetDataFromFixed16(ValueGreen))
                _return.AddRange(GetDataFromFixed16(ValueBlue))
                _return.AddRange(GetDataFromFixed16(ValueLuma))

                Return _return
            End Get
        End Property

        Public ReadOnly Property MaxGuiValue() As Double
            Get
                Return MaxValue
            End Get
        End Property

        Public ReadOnly Property MinGuiValue() As Double
            Get
                Return MinValue
            End Get
        End Property

        Private _ValueRed As Double = 0
        Public Property ValueRed(Optional autoSend As Boolean = True) As Double
            Get
                Return _ValueRed
            End Get
            Set(ByVal inValue As Double)
                If inValue < MinValue Then inValue = MinValue
                If inValue > MaxValue Then inValue = MaxValue
                _ValueRed = Math.Round(inValue, 3)
                RaisePropertyChanged("ValueRed")
                RaisePropertyChanged("ValueGuiRed")
                If autoSend Then SendValue()
            End Set
        End Property

        Private _ValueGreen As Double = 0
        Public Property ValueGreen(Optional autoSend As Boolean = True) As Double
            Get
                Return _ValueGreen
            End Get
            Set(ByVal inValue As Double)
                If inValue < MinValue Then inValue = MinValue
                If inValue > MaxValue Then inValue = MaxValue
                _ValueGreen = Math.Round(inValue, 3)
                RaisePropertyChanged("ValueGreen")
                RaisePropertyChanged("ValueGuiGreen")
                If autoSend Then SendValue()
            End Set
        End Property

        Private _ValueBlue As Double = 0
        Public Property ValueBlue(Optional autoSend As Boolean = True) As Double
            Get
                Return _ValueBlue
            End Get
            Set(ByVal inValue As Double)
                If inValue < MinValue Then inValue = MinValue
                If inValue > MaxValue Then inValue = MaxValue
                _ValueBlue = Math.Round(inValue, 3)
                RaisePropertyChanged("ValueBlue")
                RaisePropertyChanged("ValueGuiBlue")
                If autoSend Then SendValue()
            End Set
        End Property

        Private _ValueLuma As Double = 0
        Public Property ValueLuma(Optional autoSend As Boolean = True) As Double
            Get
                Return _ValueLuma
            End Get
            Set(ByVal inValue As Double)
                If inValue < MinValue Then inValue = MinValue
                If inValue > MaxValue Then inValue = MaxValue
                _ValueLuma = Math.Round(inValue, 3)
                RaisePropertyChanged("ValueLuma")
                RaisePropertyChanged("ValueGuiLuma")
                If autoSend Then SendValue()
            End Set
        End Property

        Public Property ValueGuiRed() As Double
            Get
                Return ValueRed()
            End Get
            Set(ByVal inValue As Double)
                ValueRed(True) = inValue
            End Set
        End Property

        Public Property ValueGuiGreen() As Double
            Get
                Return ValueGreen()
            End Get
            Set(ByVal inValue As Double)
                ValueGreen(True) = inValue
            End Set
        End Property

        Public Property ValueGuiBlue() As Double
            Get
                Return ValueBlue()
            End Get
            Set(ByVal inValue As Double)
                ValueBlue(True) = inValue
            End Set
        End Property

        Public Property ValueGuiLuma() As Double
            Get
                Return ValueLuma()
            End Get
            Set(ByVal inValue As Double)
                ValueLuma(True) = inValue
            End Set
        End Property

#Region " IncreaseValues Command"

        Public ReadOnly Property IncreaseValuesCommand() As RelayCommand
            Get
                Return New RelayCommand(AddressOf IncreaseValuesAction)
            End Get
        End Property

        Private Sub IncreaseValuesAction()
            ValueRed(False) = ValueRed() + 0.01
            ValueGreen(False) = ValueGreen() + 0.01
            ValueBlue(False) = ValueBlue() + 0.01
            ValueLuma(True) = ValueLuma() + 0.01
        End Sub

#End Region

#Region " DecreaseValues Command"

        Public ReadOnly Property DecreaseValuesCommand() As RelayCommand
            Get
                Return New RelayCommand(AddressOf DecreaseValuesAction)
            End Get
        End Property

        Private Sub DecreaseValuesAction()
            ValueRed(False) = ValueRed() - 0.01
            ValueGreen(False) = ValueGreen() - 0.01
            ValueBlue(False) = ValueBlue() - 0.01
            ValueLuma(True) = ValueLuma() - 0.01
        End Sub

#End Region

    End Class
End Namespace