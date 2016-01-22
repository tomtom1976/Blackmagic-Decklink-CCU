Imports System
Namespace Devices
    Public Class MidiDevice

        Property PortId As Integer
        Property ClientId As Integer
        Property DisplayValue As String

        ReadOnly Property PortName As String
            Get
                Return (New CannedBytes.Midi.MidiOutPortCapsCollection)(PortId).Name
            End Get
        End Property

        Private _midiSender As New MidiSysExSender

        Public Sub Send(strDataOn As String, Optional strDataOff As String = "", Optional delay As Integer = 0)
            _midiSender.Open(PortId)
            Dim sendeThread As New Threading.Thread(AddressOf SendAction)
            sendeThread.Start(New SendeData With {.DataOn = strDataOn, .DataOff = strDataOff, .Delay = delay})
        End Sub

        Private Sub SendAction(sendeData As SendeData)
            Try
                Dim bufferOnData As Byte() = Hex2ByteArray(sendeData.DataOn)
                Dim bufferOn As New MidiSysExBuffer(bufferOnData.Count())
                bufferOn.Stream.Write(bufferOnData, 0, bufferOnData.Count())
                _midiSender.Send(bufferOn)

                If sendeData.DataOff <> "" Then
                    System.Threading.Thread.Sleep(sendeData.Delay)
                    Dim bufferOffData As Byte() = Hex2ByteArray(sendeData.DataOff)
                    Dim bufferOff As New MidiSysExBuffer(bufferOffData.Count())
                    bufferOff.Stream.Write(bufferOffData, 0, bufferOffData.Count())
                    _midiSender.Send(bufferOff)
                End If
            Catch ex As Exception
            End Try
        End Sub

        Private Function Hex2ByteArray(data As String) As Byte()
            Try

                Dim hexString As String = data.Replace(" ", "")
                Dim length As Integer = hexString.Length
                Dim upperBound As Integer = length \ 2
                If length Mod 2 = 0 Then
                    upperBound -= 1
                Else
                    hexString = "0" & hexString
                End If
                Dim bytes(upperBound) As Byte
                For i As Integer = 0 To upperBound
                    bytes(i) = Convert.ToByte(hexString.Substring(i * 2, 2), 16)
                Next
                Return bytes
            Catch ex As Exception
            End Try
        End Function

        Public ReadOnly Property DisplayName As String
            Get
                Return (PortName & " Client-Id: " & ClientId & " | " & DisplayValue)
            End Get
        End Property

        Private Class SendeData
            Property DataOn As String
            Property DataOff As String
            Property Delay As Integer
        End Class

    End Class
End Namespace