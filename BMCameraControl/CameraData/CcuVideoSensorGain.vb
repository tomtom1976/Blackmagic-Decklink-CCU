Imports GalaSoft.MvvmLight.Command

Namespace CameraData
    Public Class CcuVideoSensorGain
        Inherits CameraControl

        Public Enum EmGain As UInt32
            Gain01 = 1
            Gain02 = 2
            Gain04 = 4
            Gain08 = 8
            Gain16 = 16
        End Enum

        Public Sub New(cameraNumber As Integer)
            Destination = cameraNumber
            Length = 5
            Catergory = 1
            Parameter = 1
            Type = EmType.SignedByte
            Operation = EmOperation.SetValue
        End Sub

        Public Sub New(cameraNumber As Integer, decklinkControl As DecklinkClass)
            MyClass.New(cameraNumber)
            Me.DecklickControl = decklinkControl
        End Sub


        Protected Overrides ReadOnly Property VancDataValue As System.Collections.Generic.List(Of UInteger)
            Get
                Return GetDataFromInt8(Me.Value)
            End Get
        End Property

        Private _value As EmGain = EmGain.Gain02
        Public Property Value() As EmGain
            Get
                Return _value
            End Get
            Set(ByVal inValue As EmGain)
                _value = inValue
                RaisePropertyChanged("Value")
                SendValue()
            End Set
        End Property

        Public ReadOnly Property ListOfValues() As Dictionary(Of String, EmGain)
            Get
                Dim _return As New Dictionary(Of String, EmGain)

                _return.Add("0dB", EmGain.Gain02)
                _return.Add("6dB", EmGain.Gain04)
                _return.Add("12dB", EmGain.Gain08)
                _return.Add("18dB", EmGain.Gain16)
                Return _return
            End Get
        End Property

#Region " SetVideoGain Command"

        Public ReadOnly Property SetVideoGainCommand() As RelayCommand(Of Integer)
            Get
                Return New RelayCommand(Of Integer)(AddressOf SetVideoGainAction)
            End Get
        End Property

        Private Sub SetVideoGainAction(inValue As Integer)
            Value = inValue
        End Sub

#End Region


    End Class
End Namespace