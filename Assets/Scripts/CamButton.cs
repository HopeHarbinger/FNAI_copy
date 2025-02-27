using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamButton : MonoBehaviour
{
    CameraManager cameraManager;

    [SerializeField]
    int CamNumber;

    private void Awake() {
        cameraManager = FindObjectOfType<CameraManager>();
    }
    public void ChangeCamera() {
        cameraManager.OpenCamera(CamNumber);
    }
}
