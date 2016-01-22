Namespace CameraData

    Public Class CcuOutputOverlays
        Inherits CameraControl

        Public Sub New(cameraNumber As Integer)
            Destination = cameraNumber
            Length = 6
            Catergory = 4
            Parameter = 1
            Type = emType.Signed16BitInteger
            Operation = emOperation.SetValue
        End Sub

        Protected Overrides ReadOnly Property VancDataValue As System.Collections.Generic.List(Of UInteger)
            Get
                Dim retunList As New List(Of UInteger)
                retunList.Add(&H0)
                retunList.Add(&H0)
                Return retunList
            End Get
        End Property
    End Class

End Namespace
