using Vuforia;
using UnityEngine;
using UnityEngine.Events;

public class StateConfigurationVuforia : MonoBehaviour
{
    public VuforiaBehaviour vuforiaBehaviour;

    public bool IsTrackerPose = false;

    public UnityEvent finishPositionTracker;

    void Start()
    {
        Invoke("InitManualVuforia", 1f);

        VuforiaConfiguration.Instance.Vuforia.DelayedInitialization = false;
        vuforiaBehaviour.enabled = true;


        VuforiaARController.Instance.RegisterVuforiaStartedCallback(() => {
            SetPositionTrackerVuforia();
        });
    }

    private void InitManualVuforia()
    {
        VuforiaRuntime.Instance.InitVuforia();
    }

    /// <summary>
    /// Activar/Desactivar positional tracker
    /// </summary>
    private void SetPositionTrackerVuforia()
    {
        Debug.Log("Set position");
        PositionalDeviceTracker pdt = TrackerManager.Instance.GetTracker<PositionalDeviceTracker>();

        if (pdt != null)
        {
            if (IsTrackerPose)
                pdt.Start();
            else
                pdt.Stop();
        }

        finishPositionTracker.Invoke();
    }
}
