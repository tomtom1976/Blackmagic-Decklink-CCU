Imports GalaSoft.MvvmLight.Command

Namespace CameraData
    Public Class CcuLensFocus
        Inherits CameraControl

        Public Sub New(cameraNumber As Integer)
            Destination = cameraNumber
            Length = 6
            Catergory = 0
            Parameter = 0
            Type = EmType.Signed5X11FixedPoint
            Operation = EmOperation.OffsetOrToggleValue
        End Sub

        Public Sub New(cameraNumber As Integer, decklinkControl As DecklinkClass)
            MyClass.New(cameraNumber)
            Me.DecklickControl = decklinkControl
        End Sub


        Private _value As Double = 0

        Public Sub GoNear()
        End Sub

        Public Sub GoFar()
            Operation = EmOperation.OffsetOrToggleValue
            _value = 0.0005
            SendValue()
        End Sub

        Protected Overrides ReadOnly Property VancDataValue As System.Collections.Generic.List(Of UInteger)
            Get
                Return GetDataFromFixed16(_value)
            End Get
        End Property


        Private _ZoomValue As Integer = 50
        Public Property ZoomValue() As Integer
            Get
                Return _ZoomValue
            End Get
            Set(ByVal value As Integer)
                If value < 0 Then value = 0
                If value > 10000 Then value = 10000
                _ZoomValue = value
                _value = _ZoomValue / 10000
                Operation = EmOperation.SetValue
                SendValue()
                RaisePropertyChanged("ZoomValue")
            End Set
        End Property



#Region " Commands "

#Region " GoFar Command"

        Public ReadOnly Property GoFarCommand() As RelayCommand(Of Double)
            Get
                Return New RelayCommand(Of Double)(AddressOf GoFarAction)
            End Get
        End Property

        Private Sub GoFarAction(changeValue As Double)
            Operation = EmOperation.OffsetOrToggleValue
            _value = changeValue
            SendValue()
        End Sub

#End Region

#Region " GoNear Command"

        Public ReadOnly Property GoNearCommand() As RelayCommand(Of Double)
            Get
                Return New RelayCommand(Of Double)(AddressOf GoNearAction)
            End Get
        End Property

        Private Sub GoNearAction(changeValue As Double)
            Operation = EmOperation.OffsetOrToggleValue
            _value = changeValue * -1
            SendValue()
        End Sub

#End Region

#Region " SendFocusValue Command"

        Public ReadOnly Property SendFocusValueCommand() As RelayCommand
            Get
                Return New RelayCommand(AddressOf SendFocusValueAction)
            End Get
        End Property

        Private Sub SendFocusValueAction()
            SendValue()
        End Sub

#End Region

#Region " AutoFocus Command"

        Public ReadOnly Property AutoFocusCommand() As RelayCommand
            Get
                Return New RelayCommand(AddressOf AutoFocusAction)
            End Get
        End Property

        Private Sub AutoFocusAction()
            If Not IsNothing(DecklickControl) Then
                DecklickControl.CameraControlData.Insert(0, New CcuLensAutoFocus(Me.Destination))
            End If
        End Sub

#End Region


#End Region



    End Class
End Namespace