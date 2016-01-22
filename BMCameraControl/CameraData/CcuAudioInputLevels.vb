Namespace CameraData
    Public Class CcuAudioInputLevels
        Inherits CameraControl

        'todo:Audio InputLevels
        Public Sub New(cameraNumber As Integer)
            Destination = cameraNumber
            Length = 8
            Catergory = 2
            Parameter = 5
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
                _return.AddRange(GetDataFromFixed16(ValueCh1()))
                _return.AddRange(GetDataFromFixed16(ValueCh2()))
                Return _return
            End Get
        End Property

        Private _ValueCh1 As Double = 0.5
        Public Property ValueCh1(Optional autoSend As Boolean = True) As Double
            Get
                Return _ValueCh1
            End Get
            Set(ByVal inValue As Double)
                If inValue < 0 Then inValue = 0
                If inValue > 1 Then inValue = 1
                inValue = Math.Round(inValue, 2)
                _ValueCh1 = inValue
                RaisePropertyChanged("ValueCh1")
                RaisePropertyChanged("ValueUpdateCh1")
                If autoSend Then SendValue()
            End Set
        End Property

        Public Property ValueUpdateCh1() As Double
            Get
                Return CInt(ValueCh1 * 100)
            End Get
            Set(ByVal inValue As Double)
                ValueCh1(True) = inValue / 100
            End Set
        End Property

        Private _ValueCh2 As Double = 0.5
        Public Property ValueCh2(Optional autoSend As Boolean = True) As Double
            Get
                Return _ValueCh2
            End Get
            Set(ByVal inValue As Double)
                If inValue < 0 Then inValue = 0
                If inValue > 1 Then inValue = 1
                inValue = Math.Round(inValue, 2)
                _ValueCh2 = inValue
                RaisePropertyChanged("ValueCh2")
                RaisePropertyChanged("ValueUpdateCh2")
                If autoSend Then SendValue()
            End Set
        End Property

        Public Property ValueUpdateCh2() As Double
            Get
                Return CInt(ValueCh2 * 100)
            End Get
            Set(ByVal inValue As Double)
                ValueCh2(True) = inValue / 100
            End Set
        End Property
    End Class
End Namespace