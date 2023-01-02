using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using Cinemachine;

// This sets up the scene camera for the local player


    public class PlayerCamera : MonoBehaviour
    {
        private Camera mainCam;
        private GameObject gameCamera;
        private CinemachineVirtualCamera cineCam;

        private GameObject cinemaCamera;

        public void Start()
        {

        }

        // public override void OnStartLocalPlayer()
        // {
//             Debug.Log("Setting up camera controllers");
//             mainCam = Camera.main;
// //            gameCamera = GameObject.FindGameObjectWithTag("GameCamera");
// //            mainCam.enabled = false;
           
//             cineCam = GameObject.FindGameObjectWithTag("PlayerCamera").GetComponent<CinemachineVirtualCamera>();

//             if (isLocalPlayer)
//             {
//  //             mainCam.enabled = false; 
//  //             gameCamera.SetActive(true);
//                 // Debug.Log("Setting camera transform");
//                 mainCam.orthographic = false;
//                 // mainCam.transform.SetParent(transform);
//                 cineCam.Follow = this.transform;
//                 Debug.Log("cinemachine cam follow set up for local client");
//             }
        // }


        public void SetupPlayerCamera()
        {
            Debug.Log("Setting up camera controllers");
            mainCam = Camera.main;

           
            cineCam = GameObject.FindGameObjectWithTag("PlayerCamera").GetComponent<CinemachineVirtualCamera>();

                mainCam.orthographic = false;
                cineCam.Follow = this.transform;
                Debug.Log("cinemachine cam follow set up for local client");

        }

        // public override void OnStopLocalPlayer()
        // {
        //     if (mainCam != null)
        //     {
        //         mainCam.transform.SetParent(null);
        //         SceneManager.MoveGameObjectToScene(mainCam.gameObject, SceneManager.GetActiveScene());
        //         mainCam.orthographic = true;
        //         mainCam.transform.localPosition = new Vector3(0f, 70f, 0f);
        //         mainCam.transform.localEulerAngles = new Vector3(90f, 0f, 0f);
        //     }
        // }
    }

