Namespace CameraData
    Public Class CcuVideoExposure
        Inherits CameraControl


        Public Sub New(cameraNumber As Integer)
            Destination = cameraNumber
            Length = 6
            Catergory = 1
            Parameter = 6
            Type = EmType.Signed16BitInteger
            Operation = EmOperation.SetValue
        End Sub

        Public Sub New(cameraNumber As Integer, decklinkControl As DecklinkClass)
            MyClass.New(cameraNumber)
            Me.DecklickControl = decklinkControl
        End Sub

        Protected Overrides ReadOnly Property VancDataValue As System.Collections.Generic.List(Of UInteger)
            Get
                Return GetDataFromInt16(Value)
            End Get
        End Property

        Private _Value As Int16
        Public Property Value() As Int16
            Get
                Return _Value
            End Get
            Set(ByVal inValue As Int16)
                'todo: Beschränkung der Werte auf die möglichen Kamera
                _Value = inValue
                RaisePropertyChanged("Value")
                SendValue()
            End Set
        End Property

        Public ReadOnly Property DisplayValue As String
            Get
                'todo: Formatieren der Werte auf die möglichen Kamera
                Return Value.ToString()
            End Get
        End Property


    End Class
End Namespace