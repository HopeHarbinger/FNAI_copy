using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraManager : MonoBehaviour
{
    public Camera controlRoomCamera;
    public Camera CAM1A;
    public Camera CAM1B;
    public Camera CAM2A;
    public Camera CAM2B;
    public Camera CAM3;
    public Camera CAM4;
    public Camera CAM5;
    public Camera CAM6;

    public Camera[] cameras;

    public int currentCamera = 0;

    public GameObject VideoScreen;

    public GameObject Map;

    public Text RoomName;

    public AudioSource source;
    public AudioClip clip;

    EditValues editValues;

    private void Awake() {

        cameras = new Camera[8];

        cameras[0] = CAM1A;
        cameras[1] = CAM1B;
        cameras[2] = CAM2A;
        cameras[3] = CAM2B;
        cameras[4] = CAM3;
        cameras[5] = CAM4;
        cameras[6] = CAM5;
        cameras[7] = CAM6;

        editValues = FindObjectOfType<EditValues>();
    }

    public void OpenCamera(int delta) { 

        cameras[currentCamera].enabled = false; //Sulkee edellisen kameran, jotta ei ole ylim‰‰r‰isi‰ kameroita p‰‰ll‰.
        currentCamera = delta; //Varastoi uuden kameran.

        controlRoomCamera.enabled = false; //Poistaa valvontahuoneen kameran.
        cameras[currentCamera].enabled = true; //Laittaa uuden kameran p‰‰lle.
        
        VideoScreen.SetActive(true);
        Map.SetActive(true);

        source.PlayOneShot(clip);//static noise sound
        editValues.EffectsOn();//screen effects

        if (currentCamera == 0) {   //Tarjoaa nimet eri tiloille
            RoomName.text = "Kitchen";
        } 
        else if (currentCamera == 1) 
        {
            RoomName.text = "Cafeteria";
        }
        else if (currentCamera == 2) 
        {
            RoomName.text = "Left aisle";
        }
        else if (currentCamera == 3) 
        {
            RoomName.text = "Right aisle";
        }
        else if (currentCamera == 4) 
        {
            RoomName.text = "Cleaning aisle";
        }
        else if (currentCamera == 5) 
        {
            RoomName.text = "Storing aisle";
        }
        else if (currentCamera == 6) 
        {
            RoomName.text = "Janitor's closet";
        }
        else if (currentCamera == 7) 
        {
            RoomName.text = "Storage";
        }
    }

    public void ShutCamera()
    {
        cameras[currentCamera].enabled = false;
        editValues.EffectsOff();//screen effects off!
        
        VideoScreen.SetActive(false);
        Map.SetActive(false);
        
        controlRoomCamera.enabled = true;
    }
}
