
public delegate void AudioEventHandler();

namespace Trophies.Maptek
{
    internal interface ITrackableAudioHandler
    {
        event AudioEventHandler AudioTargetFoundStartEvent;
        event AudioEventHandler AudioTargetFoundStopEvent;
        event AudioEventHandler AudioTargetLostStartEvent;
        event AudioEventHandler AudioTargetLostStopEvent;

        void SetAudioFound(bool set = true);
        void SetAudioLost(bool set = true);

    }
}