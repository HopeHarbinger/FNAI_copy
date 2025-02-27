using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField]
    float kameranvauhti;

    [SerializeField]
    Transform Player;

    float mouseX;
    float mouseY;

    private void Update() {

        mouseX += Input.GetAxis("Mouse X") * kameranvauhti;
        mouseY += Input.GetAxis("Mouse Y") * kameranvauhti;

        mouseX = Mathf.Clamp(mouseX, -47f, 47f); //Täs on säädetty minimi/maximi-rotaatio vasemmalle ja oikeelle.
        mouseY = Mathf.Clamp(mouseY, 0f, 0f); //Täs on säädetty minimi/maximi-rotaatio ylös ja alas.

        Player.transform.rotation = Quaternion.Euler(-mouseY, mouseX, 0f);
    }
}
