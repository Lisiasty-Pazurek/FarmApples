using UnityEngine;
using Cinemachine;
using MirrorBasics;

// This sets up the scene cinemachine camera to borrow lobby menu camera and follow player gameobject

    public class PlayerCamera : MonoBehaviour
    {
        private Camera mainCam;
        private CinemachineVirtualCamera cineCam;
        public PlayerScore pScore;


         public void SetupPlayerCamera()
        {
            mainCam = Camera.main;
            pScore = GetComponent<PlayerScore>();

            if (pScore == null && mainCam != null)
            {
                cineCam = mainCam.GetComponent<CinemachineVirtualCamera>();
                mainCam.orthographic = false;
                cineCam.Follow = this.transform;
            }
            if (pScore != null && mainCam != null) 
            {
                if (pScore.isNavigator) {SetupNavigatorCamera(); return;} 
                else
                cineCam = mainCam.GetComponent<CinemachineVirtualCamera>();
                mainCam.orthographic = false;
                cineCam.Follow = this.transform;
                Debug.Log("cinemachine cam follow set up for local client");
            }
        }

        public void SetupNavigatorCamera()
        {   
            if (!pScore.isNavigator) {return;}
            mainCam.gameObject.SetActive(false);
            GameObject navCam = GameObject.FindGameObjectWithTag("NavigatorCamera");
            navCam.GetComponent<Camera>().enabled = true;
        }

        public void SetNavigator(bool state)
        {
            pScore.isNavigator = state;
        }

    }

