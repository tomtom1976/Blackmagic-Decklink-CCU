Imports System.Runtime.InteropServices

Module VancHelper

    Public Function EncodeByte(dataByte As UInt32) As UInt32
        Dim _temp As UInt32 = dataByte
        _temp = _temp Xor _temp >> 4
        _temp = _temp Xor _temp >> 2
        _temp = _temp Xor _temp >> 1
        _temp = _temp And 1
        dataByte = dataByte Or _temp << 8
        dataByte = dataByte Or ((Not _temp) And 1) << 9
        Return dataByte
    End Function

    Public Sub WriteAncDataToLuma(ByRef buffer As IntPtr, value As UInt32, dataPosition As UInteger, ByRef streamPosition As Integer)
        Select Case dataPosition Mod 3
            Case 0
                value <<= 10
                Marshal.WriteInt32(buffer, streamPosition * 4, CInt(value))
                Debug.Print(Convert.ToString(Marshal.ReadInt32(buffer, streamPosition * 4), 2))
                streamPosition += 1
            Case 1
                Marshal.WriteInt32(buffer, streamPosition * 4, CInt(value))
                Debug.Print(Convert.ToString(Marshal.ReadInt32(buffer, streamPosition * 4), 2))
            Case 2
                value <<= 20
                Marshal.WriteInt32(buffer, streamPosition * 4, Marshal.ReadInt32(buffer, streamPosition * 4) Or CInt(value))
                Debug.Print(Convert.ToString(Marshal.ReadInt32(buffer, streamPosition * 4), 2))
                streamPosition += 1
        End Select
    End Sub

End Module
