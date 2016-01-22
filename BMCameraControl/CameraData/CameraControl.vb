Imports Newtonsoft.Json

Namespace CameraData

    <JsonObject(MemberSerialization.OptIn)> _
    Public MustInherit Class CameraControl
        Inherits GalaSoft.MvvmLight.ViewModelBase

        Public Enum EmType As UInt32
            VoidOrBoolean = 0
            SignedByte = 1
            Signed16BitInteger = 2
            Signed32BitInteger = 3
            Signed64BitInteger = 4
            Utf8String = 5
            Signed5X11FixedPoint = 128
        End Enum

        Public Enum EmOperation As UInt32
            SetValue = 0
            OffsetOrToggleValue = 1
        End Enum

        Protected Destination As UInt32
        Protected Length As UInt32
        Protected Catergory As UInt32
        Protected Parameter As UInt32
        Protected Type As EmType
        Protected Operation As EmOperation

        Private _DecklickControl As DecklinkClass = Nothing
        Public Property DecklickControl() As DecklinkClass
            Get
                Return _DecklickControl
            End Get
            Set(ByVal value As DecklinkClass)
                _DecklickControl = value
            End Set
        End Property

        <JsonProperty()> _
        Public Property CameraNumber() As UInt32
            Get
                Return Destination
            End Get
            Set(ByVal inValue As UInt32)
                Destination = inValue
                RaisePropertyChanged("CameraNumber")
            End Set
        End Property


        Public ReadOnly Property VancData As List(Of UInt32)
            Get
                Dim returnList As New List(Of UInt32)

                returnList.Add(Destination)
                returnList.Add(Length)
                returnList.Add(0)
                returnList.Add(0)
                returnList.Add(Catergory)
                returnList.Add(Parameter)
                returnList.Add(Type)
                returnList.Add(Operation)

                If Not IsNothing(VancDataValue) Then
                    returnList.AddRange(VancDataValue)
                End If

                If returnList.Count Mod 4 > 0 Then
                    For i = 0 To (3 - (returnList.Count Mod 4))
                        returnList.Add(0)
                    Next
                End If

                Return returnList
            End Get
        End Property

        Protected MustOverride ReadOnly Property VancDataValue As List(Of UInt32)

        Protected Function GetDataFromInt8(value As Integer) As List(Of UInt32)
            Dim returnList As New List(Of UInt32)
            returnList.Add(value And &HFF)
            Return returnList
        End Function

        Protected Function GetDataFromInt16(value As Integer) As List(Of UInt32)
            Dim returnList As New List(Of UInt32)
            returnList.Add(value And &HFF)
            returnList.Add((value >> 8) And &HFF)
            Return returnList
        End Function

        Protected Function GetDataFromInt32(value As Integer) As List(Of UInt32)
            Dim returnList As New List(Of UInt32)
            returnList.Add(value And &HFF)
            returnList.Add((value >> 8) And &HFF)
            returnList.Add((value >> 16) And &HFF)
            returnList.Add((value >> 24) And &HFF)
            Return returnList
        End Function

        Protected Function GetDataFromInt64(value As Int64) As List(Of UInt32)
            Dim returnList As New List(Of UInt32)
            returnList.Add(value And &HFF)
            returnList.Add((value >> 8) And &HFF)
            returnList.Add((value >> 16) And &HFF)
            returnList.Add((value >> 24) And &HFF)
            returnList.Add((value >> 32) And &HFF)
            returnList.Add((value >> 40) And &HFF)
            returnList.Add((value >> 48) And &HFF)
            returnList.Add((value >> 56) And &HFF)
            Return returnList
        End Function

        Protected Function GetDataFromFixed16(value As Double) As List(Of UInt32)
            Dim negativ As Boolean
            Dim real As Integer
            Dim fraction As Integer
            Dim returnInt As Integer = 0

            If value < 0 Then
                negativ = True
            Else
                negativ = False
            End If

            real = Fix(value)

            If negativ = True Then
                fraction = Math.Round(((1 + (value - real)) * 2048), 0)
                real = &H1F Xor Fix(value)
            Else
                fraction = Fix((value - real) * 2048)
            End If

            If negativ Then
                returnInt = returnInt Or &H8000
            End If

            returnInt = returnInt Or (real << 11) Or fraction

            Dim returnList As New List(Of UInt32)
            returnList.Add(returnInt And &HFF)
            returnList.Add((returnInt >> 8) And &HFF)
            Return returnList
        End Function

        Protected Sub SendValue()
            If Not IsNothing(_DecklickControl) Then
                If _DecklickControl.IsRunning Then
                    _DecklickControl.CameraControlData.Insert(0, Me)
                End If
            End If
        End Sub

    End Class

End Namespace
