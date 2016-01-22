Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.IO
Imports CannedBytes.IO
Imports CannedBytes.Midi

Namespace Devices

    Public Class MidiSysExBuffer

        Public Const ByteSeperator As Char = " "c

        Private buffer As Byte()

        Public Sub New(capacity As Integer)
            Me.buffer = New Byte(capacity - 1) {}
            Me.m_stream = New MemoryStream(Me.buffer, True)
        End Sub

        Private m_stream As MemoryStream
        Public ReadOnly Property Stream() As Stream
            Get
                Return Me.m_stream
            End Get
        End Property

        Public Shared Function From(buffer As MidiBufferStream) As MidiSysExBuffer
            Dim length As Integer = CInt(buffer.BytesRecorded)
            Dim sysExBuffer = New MidiSysExBuffer(length)

            buffer.Position = 0

            buffer.Read(sysExBuffer.buffer, 0, length)

            Return sysExBuffer
        End Function

        Public Overrides Function ToString() As String
            Return ToString(Nothing)
        End Function

        Public Overloads Function ToString(format As String) As String
            Dim text As String = Nothing

            Select Case format
                Case "D"
                    text = ConvertToString("{0}")
                    Exit Select
                Case Else

                    text = ConvertToString("{0:X2}")
                    Exit Select
            End Select

            Return text
        End Function

        Private Function ConvertToString(format As String) As String
            Dim text As New StringBuilder()

            For Each b As Byte In Me.buffer
                If text.Length > 0 Then
                    text.Append(ByteSeperator)
                End If


                text.AppendFormat(format, b)
            Next

            Return text.ToString()
        End Function
    End Class
End Namespace