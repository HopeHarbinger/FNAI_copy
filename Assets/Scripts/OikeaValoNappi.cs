using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OikeaValoNappi : MonoBehaviour
{
    [SerializeField]
    int valonvirrankulutus = 1;

    VasenValoNappi vasenValo;
    PowerManager powerUse;

    public Light valo;
    public GameObject pimeys;

    public bool oikeaValoPäällä;//toista valokatkaisijaa varten.

    public bool lightOn = false;//mörköjä varten.

    public AudioSource sourceOikeaNappi;
    public AudioClip nappi;

    private void Awake() {
        vasenValo = FindObjectOfType<VasenValoNappi>();
        powerUse = FindObjectOfType<PowerManager>();
    }

    void OnMouseUp() {

        if (lightOn == false && vasenValo.vasenValoPäällä == false) {
            
            lightOn = true;
            oikeaValoPäällä = true;

            powerUse.AdjustUsage(valonvirrankulutus);

            valo.enabled = true;
            pimeys.SetActive(false);

            sourceOikeaNappi.Stop();//Estää overlapin
            sourceOikeaNappi.PlayOneShot(nappi);

        } else if (lightOn == true) {
            
            lightOn = false;
            oikeaValoPäällä = false;

            powerUse.AdjustUsage(-valonvirrankulutus);

            valo.enabled = false;
            pimeys.SetActive(true);

            sourceOikeaNappi.Stop();//Estää overlapin
            sourceOikeaNappi.PlayOneShot(nappi);
        }
    }
}
