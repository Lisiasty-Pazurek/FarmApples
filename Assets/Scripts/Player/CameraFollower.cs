using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using MirrorBasics;

public class CameraFollower : MonoBehaviour
{
        private Camera mainCam;
        private CinemachineVirtualCamera cineCam;
    // Start is called before the first frame update
    void Start()
    {
        cineCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CinemachineVirtualCamera>();
        mainCam.orthographic = false;
        cineCam.Follow = this.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
