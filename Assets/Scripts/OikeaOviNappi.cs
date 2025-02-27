using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OikeaOviNappi : MonoBehaviour
{
    [SerializeField]
    int ovenvirrankulutus = 2;

    PowerManager powerUse;

    public Transform ovi;
    public Light valo;

    public bool rightDoorClosed = false;

    [SerializeField]
    float speed;

    public AudioSource sourceOikeaOvi;
    public AudioClip oikeaOviKiinni;
    public AudioClip oikeaOviAuki;

    private void Awake() {
        powerUse = FindObjectOfType<PowerManager>();
    }

    private void Update() {//Vois olla siistimpi mut nvm

        Vector3 alas = Vector3.down * Time.deltaTime * speed;

        if (rightDoorClosed == true && ovi.position.y >= 4.5f) {
            ovi.position += alas;
            valo.enabled = true;
        }
        if (rightDoorClosed == false && ovi.position.y <= 12f) {
            ovi.position -= alas;
            valo.enabled = false;
        }
    }

    void OnMouseUp() {

        if (rightDoorClosed == false) {
            rightDoorClosed = true;
            powerUse.AdjustUsage(ovenvirrankulutus);

            sourceOikeaOvi.Stop();//Estää overlapin ja soittaa äänen
            sourceOikeaOvi.PlayOneShot(oikeaOviKiinni);

        } else if (rightDoorClosed == true) {
            rightDoorClosed = false;
            powerUse.AdjustUsage(-ovenvirrankulutus);

            sourceOikeaOvi.Stop();//Estää overlapin ja soittaa äänen
            sourceOikeaOvi.PlayOneShot(oikeaOviAuki);
        }
    }
}
