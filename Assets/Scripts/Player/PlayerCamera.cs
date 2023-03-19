using UnityEngine;
using Cinemachine;

// This sets up the scene cinemachine camera to borrow lobby menu camera and follow player gameobject

    public class PlayerCamera : MonoBehaviour
    {
        private Camera mainCam;
        private CinemachineVirtualCamera cineCam;

        public bool isNavigator;

         public void SetupPlayerCamera()
        {
            mainCam = Camera.main;
            if (mainCam != null) 
            {
                if (isNavigator) {SetupNavigatorCamera(); return;} 
                else
                cineCam = mainCam.GetComponent<CinemachineVirtualCamera>();
                mainCam.orthographic = false;
                cineCam.Follow = this.transform;
                Debug.Log("cinemachine cam follow set up for local client");
            }
        }

        public void SetupNavigatorCamera()
        {   
            if (!isNavigator) {return;}
            mainCam.enabled = false;
            GameObject.FindGameObjectWithTag("NavigatorCamera").SetActive(true);
        }

        public void SetNavigator(bool state)
        {
            isNavigator = state;
        }

    }

