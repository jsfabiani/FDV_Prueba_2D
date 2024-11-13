using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{

    private CinemachineVirtualCamera vcam;
    public CinemachineBrain brain;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize components.
        vcam = brain.ActiveVirtualCamera as CinemachineVirtualCamera;
    }

    // Update is called once per frame
    void Update()
    {
        // Disable and reenable main virtual camera when pressing c.
        if(Input.GetKeyDown("c"))
        {
            if (vcam.enabled)
            {
                vcam.enabled = false;
            }
            else
            {
                vcam.enabled = true;
            }
        }
    }
}
