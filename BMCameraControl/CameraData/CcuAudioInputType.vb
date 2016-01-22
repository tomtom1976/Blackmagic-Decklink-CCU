Namespace CameraData
    Public Class CcuAudioInputType
        Inherits CameraControl

        'todo:Audio InputType

        Public Enum EmInputType As Integer
            InternalMic = 0
            LineLevel = 1
            LowMicLevel = 2
            HighMicLevel = 3
        End Enum

        Public Sub New(cameraNumber As Integer)
            Destination = cameraNumber
            Length = 5
            Catergory = 2
            Parameter = 4
            Type = EmType.SignedByte
            Operation = EmOperation.SetValue
        End Sub

        Public Sub New(cameraNumber As Integer, decklinkControl As DecklinkClass)
            MyClass.New(cameraNumber)
            DecklickControl = decklinkControl
        End Sub

        Protected Overrides ReadOnly Property VancDataValue As List(Of UInteger)
            Get
                Return GetDataFromInt8(Value)
            End Get
        End Property

        Private _value As EmInputType = EmInputType.InternalMic
        Public Property Value() As EmInputType
            Get
                Return _value
            End Get
            Set(ByVal inValue As EmInputType)
                _Value = inValue
                RaisePropertyChanged("Value")
                RaisePropertyChanged("ComboBoxIndex")
                SendValue()
            End Set
        End Property

        Public Property ComboBoxIndex() As Integer
            Get
                Return CInt(Value)
            End Get
            Set(ByVal inValue As Integer)
                Value = inValue
            End Set
        End Property


    End Class
End Namespace