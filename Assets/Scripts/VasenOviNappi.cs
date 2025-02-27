using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VasenOviNappi : MonoBehaviour
{
    [SerializeField]
    int ovenvirrankulutus = 2;

    PowerManager powerUse;

    public Transform ovi;
    public Light valo;

    public bool leftDoorClosed = false;

    [SerializeField]
    float speed;

    public AudioSource sourceVasenOvi;
    public AudioClip vasenOviAuki;
    public AudioClip vasenOviKiinni;
    
    private void Awake() {
        powerUse = FindObjectOfType<PowerManager>();
    }
    private void Update() {//Vois olla siistimpi mut nvm

        Vector3 alas = Vector3.down * Time.deltaTime * speed;

        if (leftDoorClosed == true && ovi.position.y >= 4.5f) {
            ovi.position += alas;
            valo.enabled = true;
        }
        if (leftDoorClosed == false && ovi.position.y <= 12f) {
            ovi.position -= alas;
            valo.enabled = false;
        }
    }

    void OnMouseUp() {

        if (leftDoorClosed == false) {
            leftDoorClosed = true;
            powerUse.AdjustUsage(ovenvirrankulutus);

            sourceVasenOvi.Stop();//Estää overlapin ja soittaa äänen
            sourceVasenOvi.PlayOneShot(vasenOviKiinni);

        } else if (leftDoorClosed == true) {
            leftDoorClosed = false;
            powerUse.AdjustUsage(-ovenvirrankulutus);

            sourceVasenOvi.Stop();//Estää overlapin ja soittaa äänen
            sourceVasenOvi.PlayOneShot(vasenOviAuki);
        }
    }
}
