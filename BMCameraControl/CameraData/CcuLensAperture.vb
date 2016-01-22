Imports GalaSoft.MvvmLight.Command

Namespace CameraData
    Public Class CcuLensAperture
        Inherits CameraControl

        Public Sub New(cameraNumber As Integer)
            Destination = cameraNumber
            Length = 6
            Catergory = 0
            Parameter = 4
            Type = EmType.Signed16BitInteger
            Operation = EmOperation.SetValue
        End Sub

        Public Sub New(cameraNumber As Integer, decklinkControl As DecklinkClass)
            MyClass.New(cameraNumber)
            Me.DecklickControl = decklinkControl
        End Sub

        Protected Overrides ReadOnly Property VancDataValue As System.Collections.Generic.List(Of UInteger)
            Get
                Return GetDataFromInt16(Me.Value)
            End Get
        End Property

        Private _Value As Integer
        Public Property Value() As Integer
            Get
                Return _Value
            End Get
            Set(ByVal inValue As Integer)
                If inValue < 0 Then inValue = 0
                If inValue > 15 Then inValue = 15
                _Value = inValue
                RaisePropertyChanged("Value")
                SendValue()
            End Set
        End Property

#Region " Commands "

#Region " OpenAperture Command"

        Public ReadOnly Property OpenApertureCommand() As RelayCommand
            Get
                Return New RelayCommand(AddressOf OpenApertureAction)
            End Get
        End Property

        Private Sub OpenApertureAction()
            Me.Value += 1
        End Sub

#End Region

#Region " CloseAperture Command"

        Public ReadOnly Property CloseApertureCommand() As RelayCommand
            Get
                Return New RelayCommand(AddressOf CloseApertureAction)
            End Get
        End Property

        Private Sub CloseApertureAction()
            Me.Value -= 1
        End Sub

#End Region


#End Region


    End Class
End Namespace