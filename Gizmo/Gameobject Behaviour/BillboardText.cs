using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class BillboardText : MonoBehaviour
{
    public CinemachineVirtualCamera mainCVCam; // the cinemachine main virtual camera

    void LateUpdate()
    {
        transform.rotation = mainCVCam.transform.rotation; // billboard the text by setting the transform's rotation to the cinemachine main vritual camera transform's rotation
    }
}  
