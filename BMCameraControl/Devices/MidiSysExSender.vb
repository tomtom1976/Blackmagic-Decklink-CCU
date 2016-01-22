Imports System.Collections.Generic
Imports System.Threading
Imports CannedBytes.IO
Imports CannedBytes
Imports CannedBytes.Midi

Namespace Devices

    Public Class MidiSysExSender
        Inherits DisposableBase

        Private outPort As MidiOutPort

        Public Sub New()
            Me.outPort = New MidiOutPort()

            Me.outPort.BufferManager.Initialize(10, 1024)
        End Sub

        Public Sub Open(portId As Integer)
            If Not Me.outPort.IsOpen Then
                Me.outPort.Open(portId)
            End If
        End Sub

        Public Sub Close()
            If Me.outPort.IsOpen Then
                Me.outPort.Close()
            End If
        End Sub

        Public Sub SendAll(collection As IEnumerable(Of MidiSysExBuffer))
            For Each buffer As MidiSysExBuffer In collection
                Send(buffer)

                ' little pause between buffers
                Thread.Sleep(50)
            Next
        End Sub

        Public Sub Send(sysExBuffer As MidiSysExBuffer)
            Dim buffer = Me.RetrieveBuffer()

            sysExBuffer.Stream.Position = 0
            buffer.Position = 0

            StreamHelpers.CopyTo(sysExBuffer.Stream, buffer, 0)

            Me.outPort.LongData(buffer)
        End Sub

        Private Function RetrieveBuffer() As MidiBufferStream
            Dim buffer = Me.outPort.BufferManager.RetrieveBuffer()

            ' brute force buffer retrieval.
            While buffer Is Nothing
                Thread.Sleep(100)

                buffer = Me.outPort.BufferManager.RetrieveBuffer()
            End While

            Return buffer
        End Function

        Protected Overrides Sub Dispose(disposeKind As DisposeObjectKind)
            Close()
            Me.outPort.Dispose()
        End Sub
    End Class
End Namespace