using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    private Cinemachine.CinemachineVirtualCamera virtCam;
    private float cameraZoom;

    // Start is called before the first frame update
    void Start()
    {
        virtCam = GetComponent<Cinemachine.CinemachineVirtualCamera>();
        cameraZoom = virtCam.GetCinemachineComponent<Cinemachine.CinemachineFramingTransposer>().m_CameraDistance;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            cameraZoom++;
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            cameraZoom--;
        }
    }
}
