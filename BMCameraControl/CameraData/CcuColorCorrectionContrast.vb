Namespace CameraData
    Public Class CcuColorCorrectionContrast
        Inherits CameraControl

        'todo:ColorCorrection Contrast

        Public MinPivotValue As Double = 0
        Public MaxPivotValue As Double = 1.0

        Public MinAdjValue As Double = 0
        Public MaxAdjValue As Double = 2.0

        Public Sub New(cameraNumber As Integer)
            Destination = cameraNumber
            Length = 8
            Catergory = 8
            Parameter = 4
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
                _return.AddRange(GetDataFromFixed16(PivotValue()))
                _return.AddRange(GetDataFromFixed16(AdjValue()))
                Return _return
            End Get
        End Property

        Public ReadOnly Property MinPivotGuiValue() As Double
            Get
                Return MinPivotValue
            End Get
        End Property

        Public ReadOnly Property MaxPivotGuiValue() As Double
            Get
                Return MaxPivotValue
            End Get
        End Property

        Public ReadOnly Property MinAdjGuiValue() As Double
            Get
                Return MinAdjValue
            End Get
        End Property

        Public ReadOnly Property MaxAdjGuiValue() As Double
            Get
                Return MaxAdjValue
            End Get
        End Property

        Private _PivotValue As Double = 1.0
        Public Property PivotValue() As Double
            Get
                Return _PivotValue
            End Get
            Set(ByVal inValue As Double)
                If inValue < MinPivotValue Then inValue = MinPivotValue
                If inValue > MaxPivotValue Then inValue = MaxPivotValue
                inValue = Math.Round(inValue, 3)
                _PivotValue = inValue
                RaisePropertyChanged("PivotValue")
                SendValue()
            End Set
        End Property

        Private _AdjValue As Double = 1.0
        Public Property AdjValue() As Double
            Get
                Return _AdjValue
            End Get
            Set(ByVal inValue As Double)
                If inValue < MinAdjValue Then inValue = MinAdjValue
                If inValue > MaxAdjValue Then inValue = MaxAdjValue
                inValue = Math.Round(inValue, 3)
                _AdjValue = inValue
                RaisePropertyChanged("AdjValue")
                SendValue()
            End Set
        End Property

    End Class
End Namespace