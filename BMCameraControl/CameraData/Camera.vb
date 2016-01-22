Imports Newtonsoft.Json

Namespace CameraData
    <JsonObject(MemberSerialization.OptIn)> _
    Public Class Camera
        Inherits GalaSoft.MvvmLight.ViewModelBase

#Region " Constructor "

        Public Sub New()
            _ccuColorCorrectionLift = New CcuColorCorrectionLift
        End Sub

        Public Sub New(cameraNumber As Integer, decklinkControl As DecklinkClass)
            Me.CameraNumber = cameraNumber

            _ccuAudioHeadphoneLevel = New CcuAudioHeadphoneLevel(cameraNumber, decklinkControl)
            _ccuAudioHeadphoneProgramMix = New CcuAudioHeadphoneProgramMix(cameraNumber, decklinkControl)
            _ccuAudioInputLevels = New CcuAudioInputLevels(cameraNumber, decklinkControl)
            _ccuAudioInputType = New CcuAudioInputType(cameraNumber, decklinkControl)
            _ccuAudioMicLevel = New CcuAudioMicLevel(cameraNumber, decklinkControl)
            _ccuAudioPhantomePower = New CcuAudioPhantomPower(cameraNumber, decklinkControl)

            _ccuColorCorrectionLift = New CcuColorCorrectionLift(cameraNumber, decklinkControl)
            _ccuColorCorrectionGamma = New CcuColorCorrectionGamma(cameraNumber, decklinkControl)
            _ccuColorCorrectionGain = New CcuColorCorrectionGain(cameraNumber, decklinkControl)
            _ccuColorCorrectionOffset = New CcuColorCorrectionOffset(cameraNumber, decklinkControl)
            _ccuColorCorrectionContrast = New CcuColorCorrectionContrast(cameraNumber, decklinkControl)
            _ccuColorCorrectionLuma = New CcuColorCorrectionLuma(cameraNumber, decklinkControl)
            _ccuColorCorrectionColor = New CcuColorCorrectionColor(cameraNumber, decklinkControl)

            _ccuLenseFocus = New CcuLensFocus(cameraNumber, decklinkControl)
            _ccuLenseAperture = New CcuLensAperture(cameraNumber, decklinkControl)
            _ccuVideoGain = New CcuVideoSensorGain(cameraNumber, decklinkControl)
            _ccuVideoWhiteBalance = New CcuVideoWhiteBalance(cameraNumber, decklinkControl)
            _ccuVideoExposure = New CcuVideoExposure(cameraNumber, decklinkControl)
        End Sub

#End Region

#Region " Properties "

        Private _cameraNumber As Integer
        <JsonProperty()> _
        Public Property CameraNumber() As Integer
            Get
                Return _cameraNumber
            End Get
            Set(ByVal value As Integer)
                _cameraNumber = value
                RaisePropertyChanged("CameraNumber")
            End Set
        End Property

        ReadOnly _ccuLenseFocus As CcuLensFocus
        Public ReadOnly Property CcuLenseFocus() As CcuLensFocus
            Get
                Return _ccuLenseFocus
            End Get
        End Property

        Private ReadOnly _ccuLenseAperture As CcuLensAperture
        Public ReadOnly Property CcuLenseAperture() As CcuLensAperture
            Get
                Return _ccuLenseAperture
            End Get
        End Property

        Private ReadOnly _ccuVideoGain As CcuVideoSensorGain
        Public ReadOnly Property CcuVideoGain() As CcuVideoSensorGain
            Get
                Return _ccuVideoGain
            End Get
        End Property

        Private ReadOnly _ccuVideoWhiteBalance As CcuVideoWhiteBalance
        Public ReadOnly Property CcuVideoWhiteBalance() As CcuVideoWhiteBalance
            Get
                Return _ccuVideoWhiteBalance
            End Get
        End Property

        Private ReadOnly _ccuVideoExposure As CcuVideoExposure
        Public ReadOnly Property CcuVideoExposure() As CcuVideoExposure
            Get
                Return _ccuVideoExposure
            End Get
        End Property

        Private ReadOnly _ccuAudioHeadphoneLevel As CcuAudioHeadphoneLevel
        Public ReadOnly Property CcuAudioHeadphoneLevel() As CcuAudioHeadphoneLevel
            Get
                Return _ccuAudioHeadphoneLevel
            End Get
        End Property

        Private ReadOnly _ccuAudioHeadphoneProgramMix As CcuAudioHeadphoneProgramMix
        Public ReadOnly Property CcuAudioHeadphoneProgramMix() As CcuAudioHeadphoneProgramMix
            Get
                Return _ccuAudioHeadphoneProgramMix
            End Get
        End Property

        Private ReadOnly _ccuAudioInputLevels As CcuAudioInputLevels
        Public ReadOnly Property CcuAudioInputLevels() As CcuAudioInputLevels
            Get
                Return _ccuAudioInputLevels
            End Get
        End Property

        Private ReadOnly _ccuAudioInputType As CcuAudioInputType
        Public ReadOnly Property CcuAudioInputType() As CcuAudioInputType
            Get
                Return _ccuAudioInputType
            End Get
        End Property

        Private ReadOnly _ccuAudioMicLevel As CcuAudioMicLevel
        Public ReadOnly Property CcuAudioMicLevel() As CcuAudioMicLevel
            Get
                Return _ccuAudioMicLevel
            End Get
        End Property

        Private ReadOnly _ccuAudioPhantomePower As CcuAudioPhantomPower
        Public ReadOnly Property CcuAudioPhantomePower() As CcuAudioPhantomPower
            Get
                Return _ccuAudioPhantomePower
            End Get
        End Property

        Private ReadOnly _ccuColorCorrectionLift As CcuColorCorrectionLift
        <JsonProperty()> _
        Public ReadOnly Property CcuColorCorrectionLift() As CcuColorCorrectionLift
            Get
                Return _ccuColorCorrectionLift
            End Get
        End Property

        Private ReadOnly _ccuColorCorrectionGamma As CcuColorCorrectionGamma
        Public ReadOnly Property CcuColorCorrectionGamma() As CcuColorCorrectionGamma
            Get
                Return _ccuColorCorrectionGamma
            End Get
        End Property

        Private ReadOnly _ccuColorCorrectionGain As CcuColorCorrectionGain
        Public ReadOnly Property CcuColorCorrectionGain() As CcuColorCorrectionGain
            Get
                Return _ccuColorCorrectionGain
            End Get
        End Property

        Private ReadOnly _ccuColorCorrectionOffset As CcuColorCorrectionOffset
        Public ReadOnly Property CcuColorCorrectionOffset() As CcuColorCorrectionOffset
            Get
                Return _ccuColorCorrectionOffset
            End Get
        End Property

        Private ReadOnly _ccuColorCorrectionContrast As CcuColorCorrectionContrast
        Public ReadOnly Property CcuColorCorrectionContrast() As CcuColorCorrectionContrast
            Get
                Return _ccuColorCorrectionContrast
            End Get
        End Property

        Private ReadOnly _ccuColorCorrectionLuma As CcuColorCorrectionLuma
        Public ReadOnly Property CcuColorCorrectionLuma() As CcuColorCorrectionLuma
            Get
                Return _ccuColorCorrectionLuma
            End Get
        End Property

        Private ReadOnly _ccuColorCorrectionColor As CcuColorCorrectionColor
        Public ReadOnly Property CcuColorCorrectionColor() As CcuColorCorrectionColor
            Get
                Return _ccuColorCorrectionColor
            End Get
        End Property

        Public Sub SetDecklinkControl(decklinkControl As DecklinkClass)
            '_ccuAudioHeadphoneLevel.DecklickControl = decklinkControl
            '_ccuAudioHeadphoneProgramMix.DecklickControl = decklinkControl
            '_ccuAudioInputLevels.DecklickControl = decklinkControl
            '_ccuAudioInputType.DecklickControl = decklinkControl
            '_ccuAudioMicLevel.DecklickControl = decklinkControl
            '_ccuAudioPhantomePower.DecklickControl = decklinkControl

            _ccuColorCorrectionLift.DecklickControl = decklinkControl
            '_ccuColorCorrectionGamma.DecklickControl = decklinkControl
            '_ccuColorCorrectionGain.DecklickControl = decklinkControl
            '_ccuColorCorrectionOffset.DecklickControl = decklinkControl
            '_ccuColorCorrectionContrast.DecklickControl = decklinkControl
            '_ccuColorCorrectionLuma.DecklickControl = decklinkControl
            '_ccuColorCorrectionColor.DecklickControl = decklinkControl

            '_ccuLenseFocus.DecklickControl = decklinkControl
            '_ccuLenseAperture.DecklickControl = decklinkControl
            '_ccuVideoGain.DecklickControl = decklinkControl
            '_ccuVideoWhiteBalance.DecklickControl = decklinkControl
            '_ccuVideoExposure.DecklickControl = decklinkControl


            '_ccuAudioHeadphoneLevel.CameraNumber = CameraNumber
            '_ccuAudioHeadphoneProgramMix.CameraNumber = CameraNumber
            '_ccuAudioInputLevels.CameraNumber = CameraNumber
            'ccuAudioInputType.CameraNumber = CameraNumber
            '_ccuAudioMicLevel.CameraNumber = CameraNumber
            '_ccuAudioPhantomePower.CameraNumber = CameraNumber

            _ccuColorCorrectionLift.CameraNumber = CameraNumber
            '_ccuColorCorrectionGamma.CameraNumber = CameraNumber
            '_ccuColorCorrectionGain.CameraNumber = CameraNumber
            '_ccuColorCorrectionOffset.CameraNumber = CameraNumber
            '_ccuColorCorrectionContrast.CameraNumber = CameraNumber
            '_ccuColorCorrectionLuma.CameraNumber = CameraNumber
            '_ccuColorCorrectionColor.CameraNumber = CameraNumber

            '_ccuLenseFocus.CameraNumber = CameraNumber
            '_ccuLenseAperture.CameraNumber = CameraNumber
            '_ccuVideoGain.CameraNumber = CameraNumber
            '_ccuVideoWhiteBalance.CameraNumber = CameraNumber
            '_ccuVideoExposure.CameraNumber = CameraNumber
        End Sub

#End Region

    End Class

End Namespace