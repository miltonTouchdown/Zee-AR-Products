using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Vuforia;

namespace Trophies.Trophies
{
    public class ARCameraManager : MonoBehaviour
    {

        public static ARCameraManager instance;

        public TrophiesImageTarget[] activeTargets;
        List<TrophiesImageTarget> activeTrackersList = new List<TrophiesImageTarget>();
        List<TrophiesImageTarget> trackersList = new List<TrophiesImageTarget>();

        //public enum 
        public float TimeGroupRotationAnim;
        public float SpeedGroupRotationAnim;
        public float SpeedGroupMovementAnim;

        public Transform cameraTransform;

        public GameObject CombinationAnim;

        bool onStartAnimWait;
        bool animTriggered;
        bool onAnim;
        public float animWaitTime;
        float animTimer;

        public float StartAnimWaitTime;
        float StartAnimTimer;

        AudioSource RockItSound;


        private void Awake()
        {
            StartAnimTimer = StartAnimWaitTime;
            RockItSound = GetComponent<AudioSource>();
            CombinationAnim.SetActive(false);
            animTimer = animWaitTime;
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
            instance = null;
        }
        // Use this for initialization
        void Start()
        {

        }

        void Update()
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                GoBack();
            }

            if (animTriggered)
            {
                if (onStartAnimWait)
                {
                    StartAnimTimer -= Time.deltaTime;
                    if (StartAnimTimer <= 0)
                    {
                        onStartAnimWait = false;

                        AllTargetsFound();
                    }
                    else
                    {
                        return;
                    }

                }

                animTimer -= Time.deltaTime;
                if (animTimer <= 0)
                {
                    CombinationAnim.SetActive(true);
                    foreach (TrophiesImageTarget tracker in trackersList)
                    {
                        tracker.Hide();
                        tracker.StopTracking();
                    }
                    animTriggered = false;
                }
            }
        }


        void AllTargetsFound()
        {

            //GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            //go.transform.position = Midpoint;
            //go.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            instance.RockItSound.Play();
            foreach (TrophiesImageTarget target in trackersList)
            {
                target.LookAndGoToPosition(cameraTransform.position, TimeGroupRotationAnim, SpeedGroupRotationAnim, SpeedGroupMovementAnim);
            }
            //animTriggered = true;
            //onAnim = true;
            //RockItSound.Play();

        }




        public static void AddTargetReference(TrophiesImageTarget target)
        {
            if (instance == null) return;
            instance.trackersList.Add(target);
        }

        public static void TargetFound(TrophiesImageTarget target)
        {
            if (instance == null) return;

            if (instance.activeTrackersList.Contains(target))
            {
                return;
            }
            instance.AddActivetracker(target);
            //instance.activeTrackersList.Add(target);
            if (instance.activeTrackersList.Count == 4)
            {
                Debug.Log("AllTrackersDetected");
                foreach (TrophiesImageTarget currTarget in instance.trackersList)
                {
                    currTarget.FreezeTracking();
                }
                instance.animTriggered = true;
                instance.onAnim = true;
                instance.onStartAnimWait = true;

                //instance.AllTargetsFound();
            }
        }

        public static void TargetLost(TrophiesImageTarget target)
        {
            if (instance == null) return;
            instance.RemoveActivetracker(target);
            instance.activeTrackersList.Remove(target);
        }

        public static void TargetDestroy(TrophiesImageTarget target)
        {
            if (instance != null)
            {
                instance.RemoveActivetracker(target);
                instance.activeTrackersList.Remove(target);
            }

        }

        public void GoBack()
        {
            if (onAnim)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            else
            {
                SceneManager.LoadScene(GeneralManager.SceneMenuIndex);
            }
        }

        void AddActivetracker(TrophiesImageTarget target)
        {
            activeTrackersList.Add(target);
            activeTargets = activeTrackersList.ToArray();
        }

        void RemoveActivetracker(TrophiesImageTarget target)
        {
            activeTrackersList.Remove(target);
            activeTargets = activeTrackersList.ToArray();
        }

    }
}