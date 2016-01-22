Namespace CameraData
    Public Class CcuLensAutoFocus
        Inherits CameraControl

        Public Sub New(cameraNumber As Integer)
            Destination = cameraNumber
            Length = 4
            Catergory = 0
            Parameter = 1
            Type = EmType.VoidOrBoolean
            Operation = EmOperation.SetValue
        End Sub


        Protected Overrides ReadOnly Property VancDataValue As System.Collections.Generic.List(Of UInteger)
            Get
                Return New List(Of UInteger)
            End Get
        End Property

    End Class
End Namespace