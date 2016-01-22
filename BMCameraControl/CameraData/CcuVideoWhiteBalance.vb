Imports GalaSoft.MvvmLight.Command

Namespace CameraData

    Public Class CcuVideoWhiteBalance
        Inherits CameraControl

        Public Enum EmWhiteBalance As UInt32
            Wb2500 = 2500
            Wb2800 = 2800
            Wb3000 = 3000
            Wb3200 = 3200
            Wb3400 = 3400
            Wb3600 = 3600
            Wb4000 = 4000
            Wb4500 = 4500
            Wb4800 = 4800
            Wb5000 = 5000
            Wb5200 = 5200
            Wb5400 = 5400
            Wb5600 = 5600
            Wb6000 = 6000
            Wb6500 = 6500
            Wb7000 = 7000
            Wb7500 = 7500
            Wb8000 = 8000
        End Enum

        Public Sub New(cameraNumber As Integer)
            Destination = cameraNumber
            Length = 6
            Catergory = 1
            Parameter = 2
            Type = EmType.Signed16BitInteger
            Operation = EmOperation.SetValue
        End Sub

        Public Sub New(cameraNumber As Integer, decklinkControl As DecklinkClass)
            MyClass.New(cameraNumber)
            Me.DecklickControl = decklinkControl
        End Sub

        Protected Overrides ReadOnly Property VancDataValue As List(Of UInteger)
            Get
                Return GetDataFromInt16(Value)
            End Get
        End Property

        Private _value As EmWhiteBalance = EmWhiteBalance.Wb5200
        Public Property Value() As EmWhiteBalance
            Get
                Return _value
            End Get
            Set(ByVal inValue As EmWhiteBalance)
                _value = inValue
                RaisePropertyChanged("Value")
                SendValue()
            End Set
        End Property

        Public ReadOnly Property ListOfValues() As Dictionary(Of String, EmWhiteBalance)
            Get
                Dim _return As New Dictionary(Of String, EmWhiteBalance)

                _return.Add("2500", EmWhiteBalance.Wb2500)
                _return.Add("2800", EmWhiteBalance.Wb2800)
                _return.Add("3000", EmWhiteBalance.Wb3000)
                _return.Add("3200", EmWhiteBalance.Wb3200)
                _return.Add("3400", EmWhiteBalance.Wb3400)
                _return.Add("3600", EmWhiteBalance.Wb3600)
                _return.Add("4000", EmWhiteBalance.Wb4000)
                _return.Add("4500", EmWhiteBalance.Wb4500)
                _return.Add("4800", EmWhiteBalance.Wb4800)
                _return.Add("5000", EmWhiteBalance.Wb5000)
                _return.Add("5200", EmWhiteBalance.Wb5200)
                _return.Add("5400", EmWhiteBalance.Wb5400)
                _return.Add("5600", EmWhiteBalance.Wb5600)
                _return.Add("6000", EmWhiteBalance.Wb6000)
                _return.Add("6500", EmWhiteBalance.Wb6500)
                _return.Add("7000", EmWhiteBalance.Wb7000)
                _return.Add("7500", EmWhiteBalance.Wb7500)
                _return.Add("8000", EmWhiteBalance.Wb8000)

                Return _return
            End Get
        End Property


#Region " Commands "

#Region " ToggleWbSetting Command"

        Public ReadOnly Property ToggleWbSettingCommand() As RelayCommand(Of Byte)
            Get
                Return New RelayCommand(Of Byte)(AddressOf ToggleWbSettingAction)
            End Get
        End Property

        Private Sub ToggleWbSettingAction(Optional direction As Byte = 0)
            If direction = 0 Then
                Select Case Value
                    Case EmWhiteBalance.Wb2500
                        Value = EmWhiteBalance.Wb2800
                    Case EmWhiteBalance.Wb2800
                        Value = EmWhiteBalance.Wb3000
                    Case EmWhiteBalance.Wb3000
                        Value = EmWhiteBalance.Wb3200
                    Case EmWhiteBalance.Wb3200
                        Value = EmWhiteBalance.Wb3400
                    Case EmWhiteBalance.Wb3400
                        Value = EmWhiteBalance.Wb3600
                    Case EmWhiteBalance.Wb3600
                        Value = EmWhiteBalance.Wb4000
                    Case EmWhiteBalance.Wb4000
                        Value = EmWhiteBalance.Wb4500
                    Case EmWhiteBalance.Wb4500
                        Value = EmWhiteBalance.Wb4800
                    Case EmWhiteBalance.Wb4800
                        Value = EmWhiteBalance.Wb5000
                    Case EmWhiteBalance.Wb5000
                        Value = EmWhiteBalance.Wb5200
                    Case EmWhiteBalance.Wb5200
                        Value = EmWhiteBalance.Wb5400
                    Case EmWhiteBalance.Wb5400
                        Value = EmWhiteBalance.Wb5600
                    Case EmWhiteBalance.Wb5600
                        Value = EmWhiteBalance.Wb6000
                    Case EmWhiteBalance.Wb6000
                        Value = EmWhiteBalance.Wb6500
                    Case EmWhiteBalance.Wb6500
                        Value = EmWhiteBalance.Wb7000
                    Case EmWhiteBalance.Wb7000
                        Value = EmWhiteBalance.Wb7500
                    Case EmWhiteBalance.Wb7500
                        Value = EmWhiteBalance.Wb8000
                    Case EmWhiteBalance.Wb8000
                        Value = EmWhiteBalance.Wb8000
                End Select
            Else
                Select Case Value
                    Case EmWhiteBalance.Wb2500
                        Value = EmWhiteBalance.Wb2500
                    Case EmWhiteBalance.Wb2800
                        Value = EmWhiteBalance.Wb2500
                    Case EmWhiteBalance.Wb3000
                        Value = EmWhiteBalance.Wb2800
                    Case EmWhiteBalance.Wb3200
                        Value = EmWhiteBalance.Wb3000
                    Case EmWhiteBalance.Wb3400
                        Value = EmWhiteBalance.Wb3200
                    Case EmWhiteBalance.Wb3600
                        Value = EmWhiteBalance.Wb3400
                    Case EmWhiteBalance.Wb4000
                        Value = EmWhiteBalance.Wb3600
                    Case EmWhiteBalance.Wb4500
                        Value = EmWhiteBalance.Wb4000
                    Case EmWhiteBalance.Wb4800
                        Value = EmWhiteBalance.Wb4500
                    Case EmWhiteBalance.Wb5000
                        Value = EmWhiteBalance.Wb4800
                    Case EmWhiteBalance.Wb5200
                        Value = EmWhiteBalance.Wb5000
                    Case EmWhiteBalance.Wb5400
                        Value = EmWhiteBalance.Wb5200
                    Case EmWhiteBalance.Wb5600
                        Value = EmWhiteBalance.Wb5400
                    Case EmWhiteBalance.Wb6000
                        Value = EmWhiteBalance.Wb5600
                    Case EmWhiteBalance.Wb6500
                        Value = EmWhiteBalance.Wb6000
                    Case EmWhiteBalance.Wb7000
                        Value = EmWhiteBalance.Wb6500
                    Case EmWhiteBalance.Wb7500
                        Value = EmWhiteBalance.Wb7000
                    Case EmWhiteBalance.Wb8000
                        Value = EmWhiteBalance.Wb7500
                End Select
            End If

        End Sub

#End Region


#End Region

    End Class

End Namespace
